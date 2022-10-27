using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;
public class PriceSchema : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [StringLength(50)]
    public string? Name { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
    public int RateTypeEnumId { get; set; }
    [StringLength(20)]
    public string? BaseCatPax { get; set; }
    public PriceSchema(int mandantId, string? name, string? description, int rateTypeEnumId, string? baseCatPax)
    {
        MandantId = mandantId;
        Name = name;
        Description = description;
        RateTypeEnumId = rateTypeEnumId;
        BaseCatPax = baseCatPax;
    }

    public PriceSchema Update(string name, string description, int rateTypeEnumId, string baseCatPax)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        RateTypeEnumId = rateTypeEnumId;
        if (baseCatPax is not null && BaseCatPax?.Equals(baseCatPax) is not true) BaseCatPax = baseCatPax;
        return this;
    }
}
