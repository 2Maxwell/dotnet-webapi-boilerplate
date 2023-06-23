using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.VipStates;
public class CreateVipStatusRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Arrival { get; set; }
    public string? Daily { get; set; }
    public string? Departure { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
}

public class CreateVipStatusRequestValidator : CustomValidator<CreateVipStatusRequest>
{
    public CreateVipStatusRequestValidator(IReadRepository<VipStatus> repository, IStringLocalizer<CreateVipStatusRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Kz)
            .MaximumLength(10);
        RuleFor(x => x.Arrival)
            .MaximumLength(150);
        RuleFor(x => x.Daily)
            .MaximumLength(150);
        RuleFor(x => x.Departure)
            .MaximumLength(150);
        RuleFor(x => x.ChipIcon)
            .MaximumLength(100);
        RuleFor(x => x.ChipText)
            .MaximumLength(50);
    }
}

public class CreateVipStatusRequestHandler : IRequestHandler<CreateVipStatusRequest, int>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Domain.Environment.VipStatus> _repository;

    public CreateVipStatusRequestHandler(IRepositoryWithEvents<Domain.Environment.VipStatus> repository) => _repository = repository;

    public async Task<int> Handle(CreateVipStatusRequest request, CancellationToken cancellationToken)
    {
        var vipStatus = new VipStatus(
            request.MandantId,
            request.Name,
            request.Kz,
            request.Arrival,
            request.Daily,
            request.Departure,
            request.ChipIcon,
            request.ChipText
            );

        await _repository.AddAsync(vipStatus, cancellationToken);

        return vipStatus.Id;
    }
}