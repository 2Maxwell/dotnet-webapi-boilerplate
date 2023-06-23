using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace FSH.WebApi.Domain.Accounting;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class Journal : AuditableEntity<int>, IAggregateRoot
{
    public Journal(int mandantId, int journalIdMandant, int bookingIdMandant, DateTime journalDate, DateTime hotelDate, int? reservationId, string name, decimal amount, decimal price, bool debit, int itemId, int itemNumber, string source, int? bookingLineNumberId, int taxId, decimal taxRate, int state, int? referenceId, int? kasseId)
    {
        MandantId = mandantId;
        JournalIdMandant = journalIdMandant;
        BookingIdMandant = bookingIdMandant;
        JournalDate = journalDate;
        HotelDate = hotelDate;
        ReservationId = reservationId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Amount = amount;
        Price = price;
        Debit = debit;
        ItemId = itemId;
        ItemNumber = itemNumber;
        Source = source ?? throw new ArgumentNullException(nameof(source));
        BookingLineNumberId = bookingLineNumberId;
        TaxId = taxId;
        TaxRate = taxRate;
        State = state;
        ReferenceId = referenceId;
        KasseId = kasseId;
    }

    [Required]
    public int MandantId { get; set; }
    [Required]
    public int JournalIdMandant { get; set; }
    [Required]
    public int BookingIdMandant { get; set; }
    [Required]
    public DateTime JournalDate { get; set; }
    [Required]
    public DateTime HotelDate { get; set; }
    public int? ReservationId { get; set; }
    [Required]
    [StringLength(150)]
    public string Name { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public decimal Price { get; set; }
    public decimal PriceTotal => Price * Amount;
    public bool Debit { get; set; }
    [Required]
    public int ItemId { get; set; }
    [Required]
    public int ItemNumber { get; set; }
    [Required]
    [StringLength(100)]
    public string Source { get; set; } // Vorgang; PackageKz = P:Kz
    public int? BookingLineNumberId { get; set; } // Id für zusammengehörige Zeilen
    [Required]
    public int TaxId { get; set; }
    [Required]
    public decimal TaxRate { get; set; }
    public int State { get; set; }
    public int? ReferenceId { get; set; }
    public int? KasseId { get; set; }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
