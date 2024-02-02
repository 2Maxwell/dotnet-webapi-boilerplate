using FSH.WebApi.Application.General.Appointments;
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
    public int? AppointmentId { get; set; }
    public int AppointmentTargetEnum { get; set; }
    public PackageExtendedStateEnum PackageExtendedStateEnum { get; set; }
    public int? SourceId { get; set; }
    public AppointmentDto? AppointmentDto { get; set; }
}
