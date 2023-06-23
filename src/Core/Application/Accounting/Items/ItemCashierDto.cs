namespace FSH.WebApi.Application.Accounting.Items;
public class ItemCashierDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ItemNumber { get; set; }
    public int ItemGroupId { get; set; }
    public decimal Price { get; set; }
    public int TaxId { get; set; }
    public decimal TaxRate { get; set; }
    public int InvoicePosition { get; set; }
}
