using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;

public class BookingLine
{
    [Required]
    public int Id { get; set; }
    [DataType(DataType.Date)]
    public DateTime? DateBooking { get; set; }
    [StringLength(100)]
    public string? Name { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public decimal PriceTotal => Price * Amount;
    public int ItemId { get; set; }
    public int ItemNumber { get; set; }
    [StringLength(50)]
    public string? ItemName { get; set; }
    [StringLength(30)]
    public string? Source { get; set; } // PackageKz = P:Kz
    public int BookingLineNumberId { get; set; } // Id für zusammengehörige Zeilen
    public int TaxId { get; set; }
    public decimal TaxRate { get; set; }
    public int InvoicePos { get; set; }
}