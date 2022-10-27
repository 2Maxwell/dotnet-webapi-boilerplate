using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.PriceSchemas;

public class CreatePriceSchemaRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int RateTypeEnumId { get; set; }
    public string? BaseCatPax { get; set; }
    public List<PriceSchemaDetailDto> PriceSchemaDetails { get; set; } = new();
}

public class CreatePriceSchemaRequestValidator : CustomValidator<CreatePriceSchemaRequest>
{
    public CreatePriceSchemaRequestValidator(IReadRepository<PriceSchema> repository, IStringLocalizer<CreatePriceSchemaRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (priceSchema, name, ct) => await repository.GetBySpecAsync(new PriceSchemaByNameSpec(name, priceSchema.MandantId), ct) is null)
            .WithMessage((_, name) => string.Format(localizer["packageName.alreadyexists"], name));

        RuleFor(x => x.Description)
            .MaximumLength(200);

        RuleFor(x => x.BaseCatPax)
            .NotEmpty();
    }

}

public class PriceSchemaByNameSpec : Specification<PriceSchema>, ISingleResultSpecification
{
    public PriceSchemaByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CreatePriceSchemaRequestHandler : IRequestHandler<CreatePriceSchemaRequest, int>
{
    private readonly IRepository<PriceSchema> _repository;
    private readonly IRepository<PriceSchemaDetail> _priceSchemaDetailRepository;

    public CreatePriceSchemaRequestHandler(IRepository<PriceSchema> repository, IRepository<PriceSchemaDetail> priceSchemaDetailRepository)
    {
        _repository = repository;
        _priceSchemaDetailRepository = priceSchemaDetailRepository;
    }

    public async Task<int> Handle(CreatePriceSchemaRequest request, CancellationToken cancellationToken)
    {
        var priceSchema = new PriceSchema(
        request.MandantId, request.Name, request.Description, request.RateTypeEnumId, request.BaseCatPax);
        priceSchema.DomainEvents.Add(EntityCreatedEvent.WithEntity(priceSchema));
        await _repository.AddAsync(priceSchema, cancellationToken);

        if (request.PriceSchemaDetails != null && request.PriceSchemaDetails.Count() > 0)
        {
            foreach (PriceSchemaDetailDto priceSchemaDetailDto in request.PriceSchemaDetails)
            {
                PriceSchemaDetail p = new();
                p.PriceSchemaId = priceSchema.Id;
                p.TargetCatPax = priceSchemaDetailDto.TargetCatPax;
                p.TargetDifference = priceSchemaDetailDto.TargetDifference;
                p.DomainEvents.Add(EntityCreatedEvent.WithEntity(p));
                await _priceSchemaDetailRepository.AddAsync(p, cancellationToken);
            }
        }

        return priceSchema.Id;
    }
}