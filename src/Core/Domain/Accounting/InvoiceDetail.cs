using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;

public class InvoiceDetail : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int InvoiceId { get; set; }
    [Required]
    public int InvoiceIdMandant { get; set; }
    [Required]
    public int MandantId { get; set; }
    public int BookingId { get; set; }
    [Required]
    public DateTime BookingDate { get; set; }
    [Required]
    public DateTime HotelDate { get; set; }
    public int? ReservationId { get; set; }
    [Required]
    [StringLength(150)]
    public string? Name { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public decimal Price { get; set; }
    public bool Debit { get; set; }
    [Required]
    public int ItemId { get; set; }
    [Required]
    public int ItemNumber { get; set; }
    [Required]
    [StringLength(100)]
    public string? Source { get; set; } // Vorgang Cashier, NightAudit, Depositzahlung, ... ; PackageKz = P:Kz
    [StringLength(30)]
    public string? BookingLineNumberId { get; set; } // Id für zusammengehörige Zeilen
    [Required]
    public int TaxId { get; set; } // kommt von Item Datumsabhängig
    [Required]
    public decimal TaxRate { get; set; }
    public int InvoicePos { get; set; }
    public int State { get; set; }
    [StringLength(100)]
    public string? ReferenceId { get; set; } 

    public InvoiceDetail(int invoiceId, int invoiceIdMandant, int mandantId, int bookingId, DateTime bookingDate, DateTime hotelDate, int? reservationId, string? name, decimal amount, decimal price, bool debit, int itemId, int itemNumber, string? source, string? bookingLineNumberId, int taxId, decimal taxRate, int invoicePos, int state, string? referenceId)
    {
        InvoiceId = invoiceId;
        InvoiceIdMandant = invoiceIdMandant;
        MandantId = mandantId;
        BookingId = bookingId;
        BookingDate = bookingDate;
        HotelDate = hotelDate;
        ReservationId = reservationId;
        Name = name;
        Amount = amount;
        Price = price;
        Debit = debit;
        ItemId = itemId;
        ItemNumber = itemNumber;
        Source = source;
        BookingLineNumberId = bookingLineNumberId;
        TaxId = taxId;
        TaxRate = taxRate;
        InvoicePos = invoicePos;
        State = state;
        ReferenceId = referenceId;
    }
}