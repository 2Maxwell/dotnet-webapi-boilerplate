using FSH.WebApi.Domain.General;

namespace FSH.WebApi.Application.General.Appointments;
public class CreateAppointmentRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int? DurationUnitEnumId { get; set; }
    public int? Duration { get; set; }
    public string? Source { get; set; } // "PackageExtend Hotelreservierung"
    public int SourceId { get; set; } // PackageExtend
    public string? Remarks { get; set; }
    public int AppointmentTargetEnumId { get; set; } // 0 = Restaurant, Wellness (Behandlungen), Billiardtisch, Radvermietung, Tee Time, ...
    public bool Done { get; set; }
    public DateTime? DoneDate { get; set; }
}

public class CreateAppointmentRequestValidator : CustomValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator(IReadRepository<Appointment> repository, IStringLocalizer<CreateAppointmentRequestValidator> localizer)
    {
        RuleFor(x => x.Title)
        .NotEmpty()
        .MaximumLength(70);

        RuleFor(x => x.Start)
        .NotEmpty();

        RuleFor(x => x.End)
        .NotEmpty();

        RuleFor(x => x.Source)
        .NotEmpty()
        .MaximumLength(50);

        RuleFor(x => x.Remarks)
        .MaximumLength(250);
    }
}

public class CreateAppointmentRequestHandler : IRequestHandler<CreateAppointmentRequest, int>
{
    private readonly IRepository<Appointment> _repository;
    private readonly IStringLocalizer<CreateAppointmentRequestHandler> _localizer;

    public CreateAppointmentRequestHandler(IRepository<Appointment> repository, IStringLocalizer<CreateAppointmentRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = new Appointment(request.MandantId, request.Title, request.Start, request.End, request.DurationUnitEnumId, request.Duration, request.Source, request.SourceId, request.Remarks, request.AppointmentTargetEnumId, request.Done, request.DoneDate);

        await _repository.AddAsync(appointment, cancellationToken);

        return appointment.Id;
    }
}