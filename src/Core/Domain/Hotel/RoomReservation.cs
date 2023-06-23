using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Hotel;
public class RoomReservation : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    public int RoomId { get; set; }
    [StringLength(50)]
    public string? Name { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime Occupied { get; set; }
    public int ReservationId { get; set; }

    public RoomReservation(int mandantId, int roomId, string? name, DateTime occupied, int reservationId)
    {
        MandantId = mandantId;
        RoomId = roomId;
        Name = name;
        Occupied = occupied;
        ReservationId = reservationId;
    }
}
