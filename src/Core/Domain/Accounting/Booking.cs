using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;
public class Booking : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    public DateTime BookingDate { get; set; }
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
    public bool Debit { get; set; }
    [Required]
    public int ItemId { get; set; }
    [Required]
    public int ItemNumber { get; set; }
    [Required]
    [StringLength(100)]
    public string Source { get; set; } // Vorgang Cashier, NightAudit, Depositzahlung, ... ; PackageKz = P:Kz
    [StringLength(30)]
    public string? BookingLineNumberId { get; set; } // Id für zusammengehörige Zeilen
    [Required]
    public int TaxId { get; set; } // kommt von Item Datumsabhängig
    [Required]
    public decimal TaxRate { get; set; }
    public int InvoicePos { get; set; }
    public int State { get; set; } // 1 = booked, 9 = storno
    public int? InvoiceId { get; set; }
    [StringLength(100)]
    public string? ReferenceId { get; set; } // Bei SplitPrice, SplitAmount die ursprüngliche BookingId hier eintragen: SplitAmount #12345 oder SplitPrice # 12345   
    public int? KasseId { get; set; }

    public Booking(int mandantId, DateTime bookingDate, DateTime hotelDate, int? reservationId, string name, decimal amount, decimal price, bool debit, int itemId, int itemNumber, string source, string? bookingLineNumberId, int taxId, decimal taxRate, int invoicePos, int state, int? invoiceId, string? referenceId, int? kasseId)
    {
        MandantId = mandantId;
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
        InvoiceId = invoiceId;
        ReferenceId = referenceId;
        KasseId = kasseId;
    }


    // Was ist veränderbar nach der Erstellung?
    // ReservationId
    // Name
    // BookingLineNumberId ?????
    // InvoicePos
    // State
    // InvoiceId
    // ReferenceId
}
