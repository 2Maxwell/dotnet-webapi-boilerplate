using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Environment;
public class StateRegion : BaseEntity<int>, IAggregateRoot
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(5)]
    public string? Abbreviation { get; set; }
    [Required]
    public int CountryId { get; set; }

    public StateRegion(string name, string? abbreviation, int countryId)
    {
        Name = name;
        Abbreviation = abbreviation;
        CountryId = countryId;
    }
}
