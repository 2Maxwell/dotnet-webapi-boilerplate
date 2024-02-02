namespace FSH.WebApi.Application.Accounting.CashierJournals;
public class CashierJournalDto : IDto
{
    public int Id { get; set; }
    public int? InvoiceId { get; set; }
    public int? InvoiceIdMandant { get; set; }
    public DateTime JournalDate { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public decimal PriceTotal { get; set; }
    public bool Debit { get; set; }
    public int ItemId { get; set; }
    public int ItemNumber { get; set; }
    public string? Source { get; set; }
}
