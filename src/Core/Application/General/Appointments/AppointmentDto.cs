namespace FSH.WebApi.Application.General.Appointments;
public class AppointmentDto : IDto
{
    public int Id{ get; set; }
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int? DurationUnitEnumId { get; set; }
    public int? Duration { get; set; }
    public string? Source { get; set; } // "PackageExtend Hotelreservierung"
    public int SourceId { get; set; }
    public string? Remarks { get; set; }
    public int AppointmentTargetEnumId { get; set; } // Restaurant, Wellness (Behandlungen), Billiardtisch, Radvermietung, Tee Time, ...
    public bool Done { get; set; }
    public DateTime? DoneDate { get; set; }
}
