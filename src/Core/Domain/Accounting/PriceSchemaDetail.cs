using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;

public class PriceSchemaDetail : AuditableEntity<int>, IAggregateRoot
{
    public int PriceSchemaId { get; set; }
    [StringLength(20)]
    public string? TargetCatPax { get; set; }
    public decimal TargetDifference { get; set; }

    public PriceSchemaDetail()
    {

    }

    public PriceSchemaDetail(int priceSchemaId, string targetCatPax, decimal targetDifference)
    {
        PriceSchemaId = priceSchemaId;
        TargetCatPax = targetCatPax;
        TargetDifference = targetDifference;
    }

    public PriceSchemaDetail Update(string targetCatPax, decimal targetDifference)
    {
        TargetCatPax = targetCatPax;
        TargetDifference = targetDifference;
        return this;
    }

}