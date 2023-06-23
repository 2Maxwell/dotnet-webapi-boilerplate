using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Packages;

public class PackageExtendDto : IDto
{
    public int? Id { get; set; }
    public int PackageId { get; set; }
    public PackageDto PackageDto { get; set; }
    public string? ImagePath { get; set; }
    public decimal Amount { get; set; }
    public decimal? Price { get; set; }
    public DateTime? Appointment { get; set; }
    public string? AppointmentSource { get; set; }
    public int? AppointmentSourceId { get; set; }
    public PackageExtendedStateEnum PackageExtendedStateEnum { get; set; }
    public int? Duration { get; set; } // in Minuten
}
