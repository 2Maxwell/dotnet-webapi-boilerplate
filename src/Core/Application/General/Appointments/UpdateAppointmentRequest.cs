using FSH.WebApi.Domain.General;

namespace FSH.WebApi.Application.General.Appointments;
public class UpdateAppointmentRequest : IRequest<int>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int? DurationUnitEnumId { get; set; }
    public int? Duration { get; set; }
    public string? Remarks { get; set; }
    public bool Done { get; set; }
    public DateTime? DoneDate { get; set; }
}

public class UpdateAppointmentRequestValidator : CustomValidator<UpdateAppointmentRequest>
{
    public UpdateAppointmentRequestValidator(IReadRepository<Appointment> repository, IStringLocalizer<UpdateAppointmentRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(70);

        RuleFor(x => x.Start)
            .NotEmpty();

        RuleFor(x => x.End)
            .NotEmpty();

        RuleFor(x => x.Remarks)
            .MaximumLength(250);
    }
}

public class UpdateAppointmentRequestHandler : IRequestHandler<UpdateAppointmentRequest, int>
{
    private readonly IRepository<Appointment> _repository;
    private readonly IStringLocalizer<UpdateAppointmentRequestHandler> _localizer;

    public UpdateAppointmentRequestHandler(IRepository<Appointment> repository, IStringLocalizer<UpdateAppointmentRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await _repository.GetByIdAsync(request.Id, cancellationToken);

        appointment.Update(request.Title, request.Start, request.End, request.DurationUnitEnumId, request.Duration, request.Remarks, request.Done, request.DoneDate);

        await _repository.UpdateAsync(appointment, cancellationToken);

        return appointment.Id;
    }
}