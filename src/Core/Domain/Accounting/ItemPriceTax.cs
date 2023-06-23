using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;
public class ItemPriceTax : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    public int ItemId { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int TaxId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime? Start { get; set; }
    [DataType(DataType.Date)]
    public DateTime? End { get; set; }

    public ItemPriceTax(int mandantId, int itemId, decimal price, int taxId, DateTime? start, DateTime? end)
    {
        MandantId = mandantId;
        ItemId = itemId;
        Price = price;
        TaxId = taxId;
        Start = start;
        End = end;
    }

    public ItemPriceTax Update(decimal price, int taxId, DateTime? start, DateTime? end)
    {
        Price = price;
        TaxId = taxId;
        Start = start;
        End = end;
        return this;
    }
}
