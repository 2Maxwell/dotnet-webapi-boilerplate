using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;

public class CashierJournal : AuditableEntity<int>, IAggregateRoot
{
    public CashierJournal(int mandantId, int journalId, int journalIdMandant, int bookingId, int? invoiceId, int? invoiceIdMandant, DateTime journalDate, DateTime hotelDate, string name, decimal amount, decimal price, bool debit, int itemId, int itemNumber, string? source, int state, int? kasseId)
    {
        MandantId = mandantId;
        JournalId = journalId;
        JournalIdMandant = journalIdMandant;
        BookingId = bookingId;
        InvoiceId = invoiceId;
        InvoiceIdMandant = invoiceIdMandant;
        JournalDate = journalDate;
        HotelDate = hotelDate;
        Name = name;
        Amount = amount;
        Price = price;
        Debit = debit;
        ItemId = itemId;
        ItemNumber = itemNumber;
        Source = source;
        State = state;
        KasseId = kasseId;
    }

    [Required]
    public int MandantId { get; set; }
    [Required]
    public int JournalId { get; set; }
    [Required]
    public int JournalIdMandant { get; set; }
    public int BookingId { get; set; }
    public int? InvoiceId { get; set; }
    public int? InvoiceIdMandant { get; set; }
    [Required]
    public DateTime JournalDate { get; set; }
    [Required]
    public DateTime HotelDate { get; set; }
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
    public string? Source { get; set; } // Vorgang; PackageKz = P:Kz
    public int State { get; set; }
    public int? KasseId { get; set; }

}