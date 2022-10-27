namespace FSH.WebApi.Application.Accounting.Items;

public class ItemDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ItemNumber { get; set; }
    public int TaxId { get; set; }
    public decimal Price { get; set; }
    public int ItemGroupId { get; set; }
    public bool Automatic { get; set; }
    public int MandantId { get; set; }
}
