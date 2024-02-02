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
    public int? AppointmentId { get; set; }
    public int AppointmentTargetEnum { get; set; } // 0 = Restaurant, Wellness (Behandlungen), Billiardtisch, Radvermietung, Tee Time, ...
    public PackageExtendedStateEnum PackageExtendedStateEnum { get; set; } // Status des Package
    public PackageExtendSourceEnum PackageExtendSourceEnum { get; set; } // HotelReservation, MeetingReservation, TableReservation, ...
    public int? SourceId { get; set; }

    public PackageExtend(int mandantId, int packageId, string? imagePath, decimal amount, decimal? price, int? appointmentId, int appointmentTargetEnum, PackageExtendedStateEnum packageExtendedStateEnum, PackageExtendSourceEnum packageExtendSourceEnum, int? sourceId)
    {
        MandantId = mandantId;
        PackageId = packageId;
        ImagePath = imagePath;
        Amount = amount;
        Price = price;
        AppointmentId = appointmentId;
        AppointmentTargetEnum = appointmentTargetEnum;
        PackageExtendedStateEnum = packageExtendedStateEnum;
        PackageExtendSourceEnum = packageExtendSourceEnum;
        SourceId = sourceId;
    }

    public PackageExtend Update(string? imagePath, decimal amount, decimal? price, int? appointmentId, int appointmentTargetEnum, PackageExtendedStateEnum packageExtendedStateEnum)
    {
        ImagePath = imagePath;
        Amount = amount;
        Price = price;
        AppointmentId = appointmentId;
        AppointmentTargetEnum = appointmentTargetEnum;
        PackageExtendedStateEnum = packageExtendedStateEnum;
        return this;
    }
}

// Für db:
// PackageExtended sollten auch als Vorlage für Veranstaltungen oder Events vorkonfiguriert werden können
// Beispiel Tagungspauschen: Kaffeepause, Mittagessen, Kaffeepause, Abendessen.
// Überlegung ob bei Reservierungen die Packages so wie bisher genutzt werden sollen, oder ob auch PackageExtendedVorlagen
// für die Raten genutzt werden.
// Wichtig ist der PackageType ob HotelReservation oder HotelOption.
//
// PackageExtend innerhalb einer HotelReservierung kann mit Appointment ergänzt werden so das terminierte
// Packages möglich sind.
// Appointment wiederum wird mit Appointment Target ergänzt und kann so weitere Reservierungen auslösen:
// Tischreservierung, Massagetermin, TeeTime, Fahrradverleih, ...
