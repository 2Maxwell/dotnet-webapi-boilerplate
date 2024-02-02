using FSH.WebApi.Application.Environment.Persons;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceTags;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Shop;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class ReservationDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? ResKz { get; set; } // A = Offer, P = Pending, R = Reservation, C = CheckedIn, O = CheckedOut, I = CartItem
    public int BookerId { get; set; }
    public int? GuestId { get; set; }
    public int? CompanyId { get; set; }
    public int? CompanyContactId { get; set; }
    public int? TravelAgentId { get; set; }
    public int? TravelAgentContactId { get; set; }
    public string? Persons { get; set; }
    public List<PersonShopItem>? PersonShopItems { get; set; }
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public int CategoryId { get; set; }
    public int RoomAmount { get; set; }
    public int RoomNumberId { get; set; }
    public string? RoomNumber { get; set; }
    public bool RoomFixed { get; set; }
    public int RateId { get; set; }
    public string? RatePackages { get; set; }
    public decimal LogisTotal { get; set; }
    public int BookingPolicyId { get; set; }
    public int CancellationPolicyId { get; set; }
    public bool IsGroupMaster { get; set; }
    public int GroupMasterId { get; set; }
    public string? Transfer { get; set; }
    public string? MatchCode { get; set; }
    public DateTime? OptionDate { get; set; }
    public int OptionFollowUp { get; set; } // Aktion bei erreichen des OptionDate (Delete, Request, ...)
    public string? CRSNumber { get; set; }
    public string? PaxString { get; set; } // um Pax zu erzeugen
    public Guid? CartId { get; set; }
    public DateTime? BookingDone { get; set; }
    public string? Confirmations { get; set; }
    public string? Wishes { get; set; }
    public string? RemarksHelper { get; set; }
    public PersonDto Booker { get; set; }
    public PersonDto? Guest { get; set; }
    public Company? Company { get; set; }
    public PersonDto? CompanyContact { get; set; }
    public Company? TravelAgent { get; set; }
    public PersonDto? TravelAgentContact { get; set; }
    public PriceTagDto? PriceTagDto { get; set; }
    public List<PackageExtendDto>? PackageExtendOptionDtos { get; set; }
    public List<BookingLineSummary>? BookingLineSummaries { get; set; }

}
