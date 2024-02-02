using FSH.WebApi.Domain.General;
using Mapster;

namespace FSH.WebApi.Application.General.Appointments;
public class GetAppointmentRequest : IRequest<AppointmentDto>
{
    public GetAppointmentRequest(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class GetAppointmentRequestHandler : IRequestHandler<GetAppointmentRequest, AppointmentDto>
{
    private readonly IRepository<Appointment> _repository;
    private readonly IStringLocalizer<GetAppointmentRequestHandler> _localizer;

    public GetAppointmentRequestHandler(IRepository<Appointment> repository, IStringLocalizer<GetAppointmentRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<AppointmentDto> Handle(GetAppointmentRequest request, CancellationToken cancellationToken)
    {
        AppointmentDto? appointmentDto = (await _repository.GetByIdAsync(request.Id, cancellationToken)).Adapt<AppointmentDto>();

        if (appointmentDto == null) throw new NotFoundException(string.Format(_localizer["appointment.notfound"], request.Id));

        return appointmentDto;
    }
}