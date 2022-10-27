using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Accounting;

public class TaxItem : AuditableEntity<int>, IAggregateRoot
{

    [Required]
    public int TaxId { get; set; }
    public int MandantId { get; set; }
    [DataType(DataType.Date)]
    public DateTime? Start { get; set; }
    [DataType(DataType.Date)]
    public DateTime? End { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TaxRate { get; set; }

    public TaxItem(int taxId, int mandantId, DateTime? start, DateTime? end, decimal taxRate)
    {
        TaxId = taxId;
        MandantId = mandantId;
        Start = start;
        End = end;
        TaxRate = taxRate;
    }

    public TaxItem Update(int taxId, int mandantId, DateTime? start, DateTime? end, decimal taxRate)
    {
        TaxId = taxId;
        MandantId = mandantId;
        Start = start;
        End = end;
        TaxRate = taxRate;
        return this;
    }

}