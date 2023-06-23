using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Environment;
public class Country : BaseEntity<int>, IAggregateRoot
{
    //[Key]
    //public int Id { get; set; }
    [StringLength(2)]
    public string Iso { get; set; }
    [Required]
    [StringLength(80)]
    public string Name { get; set; }
    [StringLength(3)]
    public string? Iso3 { get; set; }
    public int? NumCode { get; set; }
    public int? PhoneCode { get; set; }
    public int? NStatDE { get; set; }
    public int? NStatGroupDE { get; set; }
}
