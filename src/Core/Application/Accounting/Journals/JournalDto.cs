namespace FSH.WebApi.Application.Accounting.Journals;
public class JournalDto : IDto
{

    public int Id { get; set; }
    public int MandantId { get; set; }
    public int JournalIdMandant { get; set; }
    public int BookingId { get; set; }
    public DateTime JournalDate { get; set; }
    public DateTime HotelDate { get; set; }
    public int? ReservationId { get; set; }
    public int? InvoiceId { get; set; }
    public int? InvoiceIdMandant { get; set; }
    public string? Name { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public decimal PriceTotal => Price * Amount;
    public bool Debit { get; set; }
    public int ItemId { get; set; }
    public int ItemNumber { get; set; }
    public string? Source { get; set; } // Vorgang; PackageKz = P:Kz
    public string? BookingLineNumberId { get; set; } // Id für zusammengehörige Zeilen
    public int TaxId { get; set; }
    public decimal TaxRate { get; set; }
    public int State { get; set; }
    public string? ReferenceId { get; set; }
    public int? KasseId { get; set; }
}
