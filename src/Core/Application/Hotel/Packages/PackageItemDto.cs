namespace FSH.WebApi.Application.Hotel.Packages;

public class PackageItemDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public int ItemId { get; set; }
    public decimal Price { get; set; }
    public decimal Percentage { get; set; }
    public int PackageId { get; set; }
    public int PackageItemCoreValueEnumId { get; set; }
}
