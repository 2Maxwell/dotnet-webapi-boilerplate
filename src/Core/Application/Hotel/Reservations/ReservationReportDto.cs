namespace FSH.WebApi.Application.Hotel.Reservations;
public class ReservationReportDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? ResKz { get; set; } // A = Offer, P = Pending, R = Reservation, C = CheckedIn, O = CheckedOut, I = CartItem
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int RoomAmount { get; set; }
    public string? RoomNumber { get; set; }
    public decimal LogisTotal { get; set; }
    public int BookingPolicyId { get; set; }
    public string? BookingPolicyName { get; set; }
    public int CancellationPolicyId { get; set; }
    public string? CancellationPolicyName { get; set; }
    public bool IsGroupMaster { get; set; }
    public int GroupMasterId { get; set; }
    public string BookerName { get; set; }
    public string? GuestName { get; set; }
    public string? CompanyName { get; set; }
    public string? PaxString { get; set; }
}
