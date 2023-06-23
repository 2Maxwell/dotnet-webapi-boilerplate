using FSH.WebApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Helper;
public class PackageExtend : AuditableEntity<int>, IAggregateRoot
{

    [Required]
    public int MandantId { get; set; }
    [Required]
    public int PackageId { get; set; }
    [StringLength(200)]
    public string? ImagePath { get; set; }
    public decimal Amount { get; set; }
    public decimal? Price { get; set; }
    public DateTime? Appointment { get; set; }
    [StringLength(50)]
    public string? AppointmentSource { get; set; }
    public int? AppointmentSourceId { get; set; }
    public PackageExtendedStateEnum PackageExtendedStateEnum { get; set; } // Status des Package
    public PackageExtendSourceEnum PackageExtendSourceEnum { get; set; } // HotelReservation, MeetingReservation, TableReservation, ...
    public int? SourceId { get; set; }
    public int? Duration { get; set; } // in Minuten

    public PackageExtend(int mandantId, int packageId, string? imagePath, decimal amount, decimal? price, DateTime? appointment, string? appointmentSource, int? appointmentSourceId, PackageExtendedStateEnum packageExtendedStateEnum, PackageExtendSourceEnum packageExtendSourceEnum, int? sourceId, int? duration)
    {
        MandantId = mandantId;
        PackageId = packageId;
        ImagePath = imagePath;
        Amount = amount;
        Price = price;
        Appointment = appointment;
        AppointmentSource = appointmentSource;
        AppointmentSourceId = appointmentSourceId;
        PackageExtendedStateEnum = packageExtendedStateEnum;
        PackageExtendSourceEnum = packageExtendSourceEnum;
        SourceId = sourceId;
        Duration = duration;
    }

    public PackageExtend Update(string? imagePath, decimal amount, decimal? price, DateTime? appointment, string? appointmentSource, int? appointmentSourceId, PackageExtendedStateEnum packageExtendedStateEnum, int? duration)
    {
        ImagePath = imagePath;
        Amount = amount;
        Price = price;
        Appointment = appointment;
        AppointmentSource = appointmentSource;
        AppointmentSourceId = appointmentSourceId;
        PackageExtendedStateEnum = packageExtendedStateEnum;
        Duration = duration;
        return this;
    }


}

// Für db:
// PackageExtended sollten auch als Vorlage für Veranstaltungen oder Events vorkonfiguriert werden können
// Beispiel Tagungspauschen: Kaffeepause, Mittagessen, Kaffeepause, Abendessen.
// Überlegung ob bei Reservierungen die Packages so wie bisher genutzt werden sollen, oder ob auch PackageExtendedVorlagen
// für die Raten genutzt werden.
// Wichtig ist der PackageType ob HotelReservation oder HotelOption.
