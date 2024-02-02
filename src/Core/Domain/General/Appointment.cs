using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.General;
public class Appointment : AuditableEntity<int>, IAggregateRoot
{
    public Appointment(int mandantId, string? title, DateTime start, DateTime end, int? durationUnitEnumId, int? duration, string? source, int sourceId, string? remarks, int appointmentTargetEnumId, bool done, DateTime? doneDate)
    {
        MandantId = mandantId;
        Title = title;
        Start = start;
        End = end;
        DurationUnitEnumId = durationUnitEnumId;
        Duration = duration;
        Source = source;
        SourceId = sourceId;
        Remarks = remarks;
        AppointmentTargetEnumId = appointmentTargetEnumId;
        Done = done;
        DoneDate = doneDate;

        // Für das einbinden von weiterem Content Ressource und RessourceId verwenden
    }

    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(70)]
    public string? Title { get; set; }
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }
    public int? DurationUnitEnumId { get; set; }
    public int? Duration { get; set; }
    [StringLength(50)]
    public string? Source { get; set; } // "PackageExtend Hotelreservierung"
    public int SourceId { get; set; }
    [StringLength(250)]
    public string? Remarks { get; set; }
    public int AppointmentTargetEnumId { get; set; } // 0 = Restaurant, Wellness (Behandlungen), Billiardtisch, Radvermietung, Tee Time, ...
    public bool Done { get; set; }
    public DateTime? DoneDate { get; set; }

    public Appointment Update(string? title, DateTime start, DateTime end, int? durationUnitEnumId, int? duration, string? remarks, bool done, DateTime? doneDate)
    {
        Title = title;
        Start = start;
        End = end;
        DurationUnitEnumId = durationUnitEnumId;
        Duration = duration;
        Remarks = remarks;
        Done = done;
        DoneDate = doneDate;

        return this;
    }
}
