using FSH.WebApi.Domain.Environment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Hotel;
public class Reservation : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(1)]
    public string? ResKz { get; set; } // A = Offer, P = Pending, R = Reservation, C = CheckedIn, O = CheckedOut, I = CartItem, S = Storno
    [Required]
    public int BookerId { get; set; }
    public int? GuestId { get; set; }
    public int? CompanyId { get; set; }
    public int? CompanyContactId { get; set; }
    public int? TravelAgentId { get; set; }
    public int? TravelAgentContactId { get; set; }
    public string? Persons { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public int CategoryId { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal RoomAmount { get; set; }
    public int RoomNumberId { get; set; }
    [StringLength(25)]
    public string? RoomNumber { get; set; }
    public bool RoomFixed { get; set; }
    public int RateId { get; set; }
    [StringLength(250)]
    public string? RatePackages { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal LogisTotal { get; set; }
    public int BookingPolicyId { get; set; }
    public int CancellationPolicyId { get; set; }
    public bool IsGroupMaster { get; set; }
    public int GroupMasterId { get; set; }
    [StringLength(50)]
    public string? Transfer { get; set; }
    [StringLength(25)]
    public string? MatchCode { get; set; }
    public DateTime? OptionDate { get; set; }
    public int OptionFollowUp { get; set; } // Aktion bei erreichen des OptionDate (Delete, Request, ...)
    [StringLength(25)]
    public string? CRSNumber { get; set; }
    [StringLength(100)]
    public string? PaxString { get; set; } // um Pax zu erzeugen
    public Guid? CartId { get; set; }
    public DateTime? BookingDone { get; set; }
    public string? Confirmations { get; set; }
    [StringLength(250)]
    public string? Wishes { get; set; }
    [ForeignKey("BookerId")]
    public Person? Booker { get; set; }
    [ForeignKey("GuestId")]
    public Person? Guest { get; set; }
    [ForeignKey("CompanyId")]
    public Company? Company { get; set; }
    [ForeignKey("CompanyContactId")]
    public Person? CompanyContact { get; set; }
    [ForeignKey("TravelAgentId")]
    public Company? TravelAgent { get; set; }
    [ForeignKey("TravelAgentContactId")]
    public Person? TravelAgentContact { get; set; }

    public Reservation()
    {

    }

    public Reservation(int mandantId, string? resKz, int bookerId, int? guestId, int? companyId, int? companyContactId, int? travelAgentId, int? travelAgentContactId, string? persons, DateTime arrival, DateTime departure, int categoryId, decimal roomAmount, int roomNumberId, string? roomNumber, bool roomFixed, int rateId, string? ratePackages, decimal logisTotal, int bookingPolicyId, int cancellationPolicyId, bool isGroupMaster, int groupMasterId, string? transfer, string? matchCode, DateTime? optionDate, int optionFollowUp, string? cRSNumber, string? paxString, Guid? cartId, string? confirmations, string? wishes)
    {
        MandantId = mandantId;
        ResKz = resKz;
        BookerId = bookerId;
        GuestId = guestId;
        CompanyId = companyId;
        CompanyContactId = companyContactId;
        TravelAgentId = travelAgentId;
        TravelAgentContactId = travelAgentContactId;
        Persons = persons;
        Arrival = arrival;
        Departure = departure;
        CategoryId = categoryId;
        RoomAmount = roomAmount;
        RoomNumberId = roomNumberId;
        RoomNumber = roomNumber;
        RoomFixed = roomFixed;
        RateId = rateId;
        RatePackages = ratePackages;
        LogisTotal = logisTotal;
        BookingPolicyId = bookingPolicyId;
        CancellationPolicyId = cancellationPolicyId;
        IsGroupMaster = isGroupMaster;
        GroupMasterId = groupMasterId;
        Transfer = transfer;
        MatchCode = matchCode;
        OptionDate = optionDate;
        OptionFollowUp = optionFollowUp;
        CRSNumber = cRSNumber;
        PaxString = paxString;
        CartId = cartId;
        Confirmations = confirmations;
        Wishes = wishes;
    }

    public Reservation Update(string? resKz, int bookerId, int? guestId, int? companyId, int? companyContactId, int? travelAgentId, int? travelAgentContactId, string? persons, DateTime arrival, DateTime departure, int categoryId, decimal roomAmount, int roomNumberId, string? roomNumber, bool roomFixed, int rateId, string? ratePackages, decimal logisTotal, int bookingPolicyId, int cancellationPolicyId, bool isGroupMaster, int groupMasterId, string? transfer, string? matchCode, DateTime? optionDate, int optionFollowUp, string? cRSNumber, string? paxString, Guid? cartId, string? confirmations, string? wishes)
    {
        if (resKz is not null && ResKz?.Equals(resKz) is not true) ResKz = resKz;
        BookerId = bookerId;
        GuestId = guestId;
        CompanyId = companyId;
        CompanyContactId = companyContactId;
        TravelAgentId = travelAgentId;
        TravelAgentContactId = travelAgentContactId;
        if (persons is not null && Persons?.Equals(persons) is not true) Persons = persons;
        Arrival = arrival;
        Departure = departure;
        CategoryId = categoryId;
        RoomAmount = roomAmount;
        RoomNumberId = roomNumberId;
        RoomNumber = roomNumber;
        RoomFixed = roomFixed;
        RateId = rateId;
        RatePackages = ratePackages;
        LogisTotal = logisTotal;
        BookingPolicyId = bookingPolicyId;
        CancellationPolicyId = cancellationPolicyId;
        IsGroupMaster = isGroupMaster;
        GroupMasterId = groupMasterId;
        if (transfer is not null && Transfer?.Equals(transfer) is not true) Transfer = transfer;
        if (matchCode is not null && MatchCode?.Equals(matchCode) is not true) MatchCode = matchCode;
        OptionDate = optionDate;
        OptionFollowUp = optionFollowUp;
        CRSNumber = cRSNumber;
        if (paxString is not null && PaxString?.Equals(paxString) is not true) PaxString = paxString;
        CartId = cartId;
        Confirmations = confirmations;
        Wishes = wishes;
        return this;
    }

    public Reservation UpdateTransfer(string? transfer)
    {
        if (transfer is not null && Transfer?.Equals(transfer) is not true) Transfer = transfer;
        return this;
    }

    public Reservation UpdateBookingDone(DateTime bookingDone)
    {
        BookingDone = bookingDone;
        return this;
    }

    public Reservation CheckOut()
    {
        ResKz = "O";
        // SET Departure Time to = Time.Now
        // Departure = DateTime.Now;
        return this;
    }
}

//
// External Tables:
// Communication, Statistik (Source, SourceDetail), Notes, Logging, Todo's, PriceCats

// ? Option einpflegen in eigene Table mit speziellen Funktionen für Empfang und
// Salses & Marketing

// Checks CheckListe ob alle notwendigen Punkte erfüllt sind bsp.: BookingPolicy CreditCard
// CreditCard vorhanden - check, Deposit - Deposit eingegangen - check
