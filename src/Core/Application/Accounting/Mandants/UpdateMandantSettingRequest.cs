using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.Mandants;
public class UpdateMandantSettingRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int ForecastDays { get; set; }
    public string? DefaultTransfer { get; set; }
    public DateTime DefaultArrivalTime { get; set; }
    public DateTime DefaultDepartureTime { get; set; }
    public int DefaultLanguageId { get; set; }
    public int DefaultCountryId { get; set; }
}

public class UpdateMandantSettingRequestValidator : CustomValidator<UpdateMandantSettingRequest>
{
    public UpdateMandantSettingRequestValidator()
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.DefaultTransfer)
            .MaximumLength(70);
    }
}

public class UpdateMandantSettingRequestHandler : IRequestHandler<UpdateMandantSettingRequest, int>
{
    private readonly IRepository<MandantSetting> _repository;
    private readonly IStringLocalizer _localizer;

    public UpdateMandantSettingRequestHandler(IRepository<MandantSetting> repository, IStringLocalizer<UpdateMandantSettingRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateMandantSettingRequest request, CancellationToken cancellationToken)
    {
        var mandantSetting = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = mandantSetting ?? throw new NotFoundException(_localizer["MandantSetting {0} Not Found.", request.Id]);

        var updatedMandantSetting = mandantSetting.Update(
            request.ForecastDays,
            request.DefaultTransfer,
            request.DefaultArrivalTime,
            request.DefaultDepartureTime,
            request.DefaultLanguageId,
            request.DefaultCountryId);

        // Add Domain Events to be raised after the commit
        mandantSetting.DomainEvents.Add(EntityUpdatedEvent.WithEntity(mandantSetting));

        await _repository.UpdateAsync(updatedMandantSetting, cancellationToken);

        return request.Id;
    }
}