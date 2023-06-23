using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.CancellationPolicys;

public class CreateCancellationPolicyRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? Display { get; set; }
    public string? DisplayShort { get; set; }
    public string? DisplayHighLight { get; set; }
    public string? ConfirmationText { get; set; }
    public bool IsDefault { get; set; }
    public int FreeCancellationDays { get; set; }
    public int Priority { get; set; }
    public int NoShow { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
}

public class CreateCancellationPolicyRequestValidator : CustomValidator<CreateCancellationPolicyRequest>
{
    public CreateCancellationPolicyRequestValidator(IReadRepository<CancellationPolicy> repository, IStringLocalizer<CreateCancellationPolicyRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (cancellationPolicy, name, ct) =>
            await repository.GetBySpecAsync(new CancellationPolicyByNameSpec(name, cancellationPolicy.MandantId), ct)
            is not CancellationPolicy existingCancellationPolicy || existingCancellationPolicy.MandantId == cancellationPolicy.MandantId)
            .WithMessage((_, name) => string.Format(localizer["cancellationPolicyName.alreadyexists"], name));

        RuleFor(x => x.Kz)
            .NotEmpty()
            .MaximumLength(10)
            .MustAsync(async (cancellationPolicy, kz, ct) =>
            await repository.GetBySpecAsync(new CancellationPolicyByKzSpec(kz, cancellationPolicy.MandantId), ct) is null)
            .WithMessage((_, kz) => string.Format(localizer["cancellationPolicyKz.alreadyexists"], kz));

        RuleFor(x => x.Description)
            .MaximumLength(200);
        RuleFor(x => x.Display)
            .MaximumLength(500);
        RuleFor(x => x.DisplayShort)
            .MaximumLength(300);
        RuleFor(x => x.DisplayHighLight)
            .MaximumLength(300);
        RuleFor(x => x.ConfirmationText)
            .MaximumLength(500);
        RuleFor(x => x.ChipIcon)
            .MaximumLength(100);
        RuleFor(x => x.ChipText)
            .MaximumLength(50);
    }
}

public class CancellationPolicyByNameSpec : Specification<CancellationPolicy>, ISingleResultSpecification
{
    public CancellationPolicyByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CancellationPolicyByKzSpec : Specification<CancellationPolicy>, ISingleResultSpecification
{
    public CancellationPolicyByKzSpec(string kz, int mandantId) =>
        Query.Where(x => x.Kz == kz && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CancellationPolicyByDefaultSpec : Specification<CancellationPolicy>, ISingleResultSpecification
{
    public CancellationPolicyByDefaultSpec(bool isDefault, int mandantId) =>
        Query.Where(x => x.IsDefault && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CreateCancellationPolicyRequestHandler : IRequestHandler<CreateCancellationPolicyRequest, int>
{
    private readonly IRepository<CancellationPolicy> _repository;
    public CreateCancellationPolicyRequestHandler(IRepository<CancellationPolicy> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateCancellationPolicyRequest request, CancellationToken cancellationToken)
    {
        var cancellationPolicy = new CancellationPolicy(
            request.MandantId,
            request.Name,
            request.Kz,
            request.Description,
            request.Display,
            request.DisplayShort,
            request.DisplayHighLight,
            request.ConfirmationText,
            request.IsDefault,
            request.FreeCancellationDays,
            request.Priority,
            request.NoShow,
            request.ChipIcon,
            request.ChipText
            );
        cancellationPolicy.DomainEvents.Add(EntityCreatedEvent.WithEntity(cancellationPolicy));
        await _repository.AddAsync(cancellationPolicy, cancellationToken);

        return cancellationPolicy.Id;
    }
}
