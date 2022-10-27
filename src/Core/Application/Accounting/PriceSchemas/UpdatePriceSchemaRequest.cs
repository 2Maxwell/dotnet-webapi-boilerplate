using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Accounting.PriceSchemas;

public class UpdatePriceSchemaRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int RateTypeEnumId { get; set; }
    public string? BaseCatPax { get; set; }
    public List<PriceSchemaDetailDto> PriceSchemaDetails { get; set; } = new();
}

public class UpdatePriceSchemaRequestValidator : CustomValidator<UpdatePriceSchemaRequest>
{
    public UpdatePriceSchemaRequestValidator(IReadRepository<PriceSchema> repository, IStringLocalizer<UpdatePriceSchemaRequestValidator> localizer)
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

public class UpdatePriceSchemaRequestHandler : IRequestHandler<UpdatePriceSchemaRequest, int>
{
    private readonly IRepositoryWithEvents<PriceSchema> _repository;
    private readonly IStringLocalizer<UpdatePriceSchemaRequestHandler> _localizer;
    private readonly IRepositoryWithEvents<PriceSchemaDetail> _priceSchemaDetailRepository;

    public UpdatePriceSchemaRequestHandler(IRepositoryWithEvents<PriceSchema> repository, IRepositoryWithEvents<PriceSchemaDetail> priceSchemaDetailRepository, IStringLocalizer<UpdatePriceSchemaRequestHandler> localizer) =>
        (_repository, _priceSchemaDetailRepository, _localizer) = (repository, priceSchemaDetailRepository, localizer);

    public async Task<int> Handle(UpdatePriceSchemaRequest request, CancellationToken cancellationToken)
    {
        var priceSchema = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = priceSchema ?? throw new NotFoundException(string.Format(_localizer["priceSchema.notfound"], request.Id));
        priceSchema.Update(
            request.Name,
            request.Description,
            request.RateTypeEnumId,
            request.BaseCatPax
            );

        priceSchema.DomainEvents.Add(EntityUpdatedEvent.WithEntity(priceSchema));
        await _repository.UpdateAsync(priceSchema, cancellationToken);

        if(request.PriceSchemaDetails != null && request.PriceSchemaDetails.Count() > 0)
        {
            foreach (PriceSchemaDetailDto item in request.PriceSchemaDetails)
            {
                var priceSchemaDetail = await _priceSchemaDetailRepository.GetByIdAsync(item.Id, cancellationToken);
                if(priceSchemaDetail != null)
                {
                    // Update PriceSchemaDetail
                    priceSchemaDetail.TargetCatPax = item.TargetCatPax;
                    priceSchemaDetail.TargetDifference = item.TargetDifference;
                    priceSchemaDetail.DomainEvents.Add(EntityCreatedEvent.WithEntity(priceSchemaDetail));
                    await _priceSchemaDetailRepository.UpdateAsync(priceSchemaDetail, cancellationToken);
                }
                else
                {
                    // Create PriceSchemaDetail
                    PriceSchemaDetail p = new PriceSchemaDetail();
                    p.PriceSchemaId = item.PriceSchemaId;
                    p.TargetCatPax = item.TargetCatPax;
                    p.TargetDifference = item.TargetDifference;
                    p.DomainEvents.Add(EntityCreatedEvent.WithEntity(p));
                    await _priceSchemaDetailRepository.AddAsync(p, cancellationToken);
                }
            }
        }

        return request.Id;
    }
}