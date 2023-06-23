using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.Mandants;
public class CreateMandantSettingRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public int ForecastDays { get; set; }
    public string? DefaultTransfer { get; set; }
    public DateTime DefaultArrivalTime { get; set; }
    public DateTime DefaultDepartureTime { get; set; }
    public int DefaultLanguageId { get; set; }
    public int DefaultCountryId { get; set; }
}

public class CreateMandantSettingRequestValidator : CustomValidator<CreateMandantSettingRequest>
{
    public CreateMandantSettingRequestValidator()
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.DefaultTransfer)
            .MaximumLength(70);
    }
}

public class CreateMandantSettingRequestHandler : IRequestHandler<CreateMandantSettingRequest, int>
{
    private readonly IRepository<MandantSetting> _repository;

    public CreateMandantSettingRequestHandler(IRepository<MandantSetting> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateMandantSettingRequest request, CancellationToken cancellationToken)
    {
        var mandantSetting = new MandantSetting(
            request.MandantId,
            request.ForecastDays,
            request.DefaultTransfer,
            request.DefaultArrivalTime,
            request.DefaultDepartureTime,
            request.DefaultLanguageId,
            request.DefaultCountryId);

        mandantSetting.DomainEvents.Add(EntityCreatedEvent.WithEntity(mandantSetting));
        await _repository.AddAsync(mandantSetting, cancellationToken);
        return mandantSetting.Id;
    }

}