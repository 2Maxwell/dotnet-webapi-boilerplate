namespace FSH.WebApi.Application.Accounting.Merchandises;

public class MerchandiseDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MerchandiseNumber { get; set; }
    public int TaxId { get; set; }
    public decimal Price { get; set; }
    public int MerchandiseGroupId { get; set; }
    public bool Automatic { get; set; } = false;
}
