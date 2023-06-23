namespace FSH.WebApi.Application.Accounting.Items;

public class ItemDto : IDto
{
    public int MandantId { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ItemNumber { get; set; }
    public int ItemGroupId { get; set; }
    public bool Automatic { get; set; }
    public int InvoicePosition { get; set; }
    public int AccountNumber { get; set; }
    public List<ItemPriceTaxDto> PriceTaxesDto { get; set; } = new List<ItemPriceTaxDto>();
}
