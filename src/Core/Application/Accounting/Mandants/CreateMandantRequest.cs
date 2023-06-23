using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.Mandants;

public class CreateMandantRequest : IRequest<int>
{
    public string? Kz { get; set; }
    public string? Name { get; set; }
    public int GroupMember { get; set; }
    public bool GroupHead { get; set; }
}

public class CreateMandantRequestValidator : CustomValidator<CreateMandantRequest>
{
    public CreateMandantRequestValidator(IReadRepository<Mandant> repository, IStringLocalizer<CreateMandantRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

        RuleFor(x => x.Kz)
        .NotEmpty()
        .MaximumLength(10)
        .MustAsync(async (kz, ct) => await repository.GetBySpecAsync(new MandantByKzSpec(kz), ct) is null)
                .WithMessage((_, kz) => string.Format(localizer["mandantKz.alreadyexists"], kz));
    }
}

public class MandantByKzSpec : Specification<Mandant>, ISingleResultSpecification
{
    public MandantByKzSpec(string kz) =>
        Query.Where(x => x.Kz == kz);
}

public class CreateMandantRequestHandler : IRequestHandler<CreateMandantRequest, int>
{
    private readonly IRepository<Mandant> _repository;
    private readonly IRepository<MandantNumbers> _repositoryMandantNumbers;
    public CreateMandantRequestHandler(IRepository<Mandant> repository) => _repository = repository;

    public async Task<int> Handle(CreateMandantRequest request, CancellationToken cancellationToken)
    {
        var mandant = new Mandant(
            request.Name,
            request.Kz,
            request.GroupMember,
            request.GroupHead);

        mandant.DomainEvents.Add(EntityCreatedEvent.WithEntity(mandant));
        await _repository.AddAsync(mandant, cancellationToken);

        var mandantNumbers = new MandantNumbers(
            mandant.Id,
            10000,
            30000,
            0,
            0,
            500000);
        mandantNumbers.DomainEvents.Add(EntityCreatedEvent.WithEntity(mandantNumbers));
        await _repositoryMandantNumbers.AddAsync(mandantNumbers, cancellationToken);

        return mandant.Id;
    }
}