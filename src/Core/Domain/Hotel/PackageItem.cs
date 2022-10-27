using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Hotel;

public class PackageItem : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    [Required]
    public int ItemId { get; set; }
    public decimal Price { get; set; }
    public decimal Percentage { get; set; }
    public int PackageId { get; set; }
    public int PackageItemCoreValueEnumId { get; set; }
}