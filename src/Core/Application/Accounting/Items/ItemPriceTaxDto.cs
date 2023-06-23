namespace FSH.WebApi.Application.Accounting.Items;

public class ItemPriceTaxDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int ItemId { get; set; }
    public decimal Price { get; set; }
    public int TaxId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
}
