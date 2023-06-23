namespace FSH.WebApi.Application.Accounting.Taxes;
public class TaxDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int CountryId { get; set; }
    public string? Name { get; set; }
    public int TaxSystemEnumId { get; set; }
    public virtual List<TaxItemDto>? TaxItems { get; set; }
}
