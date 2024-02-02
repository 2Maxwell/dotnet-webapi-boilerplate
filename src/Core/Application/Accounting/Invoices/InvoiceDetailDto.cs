namespace FSH.WebApi.Application.Accounting.Invoices;
public class InvoiceDetailDto : IDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int InvoiceIdMandant { get; set; }
    public int MandantId { get; set; }
    public int BookingId { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime HotelDate { get; set; }
    public int? ReservationId { get; set; }
    public string? Name { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public bool Debit { get; set; }
    public int ItemId { get; set; }
    public int ItemNumber { get; set; }
    public string? Source { get; set; } // Vorgang Cashier, NightAudit, Depositzahlung, ... ; PackageKz = P:Kz
    public string? BookingLineNumberId { get; set; } // Id für zusammengehörige Zeilen
    public int TaxId { get; set; } // kommt von Item Datumsabhängig
    public decimal TaxRate { get; set; }
    public int InvoicePos { get; set; }
    public int State { get; set; }
    public int? ReferenceId { get; set; } // keine Ahnung was ich damit machen soll

}
