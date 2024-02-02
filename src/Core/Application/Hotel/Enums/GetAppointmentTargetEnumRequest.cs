using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;
public class GetAppointmentTargetEnumRequest : IRequest<List<AppointmentTargetEnumDto>>
{
    public int MandantId { get; set; }
}

public class GetAppointmentTargetEnumRequestHandler : IRequestHandler<GetAppointmentTargetEnumRequest, List<AppointmentTargetEnumDto>>
{
    private readonly IStringLocalizer<GetAppointmentTargetEnumRequestHandler> _localizer;
    public GetAppointmentTargetEnumRequestHandler(IStringLocalizer<GetAppointmentTargetEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<AppointmentTargetEnumDto>> Handle(GetAppointmentTargetEnumRequest request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(AppointmentTargetEnum)).Cast<AppointmentTargetEnum>().Select(e => new AppointmentTargetEnumDto
        {
            Text = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class AppointmentTargetEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}

// TODO : Muss noch umgebaut werden damit nur beim Mandanten freigegebene Shops ausgewählt werden können.