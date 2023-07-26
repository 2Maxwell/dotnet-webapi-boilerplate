namespace FSH.WebApi.Application.Accounting.Bookings;
public class BookingDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public DateTime HotelDate { get; set; }
    public int ReservationId { get; set; }
    public string? Name { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public bool Debit { get; set; }
    public int ItemId { get; set; }
    public int ItemNumber { get; set; }
    public string? Source { get; set; }
    public int BookingLineNumberId { get; set; }
    public int TaxId { get; set; }
    public decimal TaxRate { get; set; }
    public int InvoicePos { get; set; }
    public int State { get; set; }
    public int InvoiceId { get; set; }
    public int ReferenceId { get; set; }
}
