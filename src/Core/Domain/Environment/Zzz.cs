using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Environment;

public class Zzz : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(50)]
    public string? Name { get; set; }
    [Required]
    [StringLength(10)]
    public string? Kz { get; set; }
    [Required]
    [StringLength(200)]
    public string? Description { get; set; }
    [Required]
    [StringLength(200)]
    public string? Display { get; set; }

    public Zzz(int mandantId, string? name, string? kz, string? description, string? display)
    {
        MandantId = mandantId;
        Name = name;
        Kz = kz;
        Description = description;
        Display = display;
    }

    public Zzz Update(string? name, string? kz, string? description, string? display)
    {
        Name = name;
        Kz = kz;
        Description = description;
        Display = display;
        return this;
    }

}
