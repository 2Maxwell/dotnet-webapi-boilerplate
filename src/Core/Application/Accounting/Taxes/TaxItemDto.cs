namespace FSH.WebApi.Application.Accounting.Taxes;
public class TaxItemDto : IDto
{
    public int Id { get; set; }
    public int TaxId { get; set; }
    public int MandantId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public decimal TaxRate { get; set; }
}
