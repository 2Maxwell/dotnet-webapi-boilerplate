using FSH.WebApi.Application.Environment.Companys;
using FSH.WebApi.Application.Environment.Persons;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class ReservationListDto : IDto
{
    public int Id { get; set; }
    public string? ResKz { get; set; } // A = Offer, P = Pending, R = Reservation, C = CheckedIn, O = CheckedOut, I = CartItem
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public int CategoryId { get; set; }
    public int RoomAmount { get; set; }
    public string? RoomNumber { get; set; }
    public bool IsGroupMaster { get; set; }
    public int GroupMasterId { get; set; }
    public int? BookerId { get; set; }
    public string? BookerName { get; set; }
    public string? BookerFirstName { get; set; }
    public string? BookerTitle { get; set; }
    public string? BookerSalutationName { get; set; }
    public int? GuestId { get; set; }
    public string? GuestName { get; set; }
    public string? GuestFirstName { get; set; }
    public string? GuestTitle { get; set; }
    public string? GuestSalutationName { get; set; }
    public int? CompanyId { get; set; }
    public string? CompanyName1 { get; set; }
    public int? TravelAgentId { get; set; }
    public string? TravelAgentName1 { get; set; }
    public string? PaxString { get; set; }
    public int BookingPolicyId { get; set; }
}
