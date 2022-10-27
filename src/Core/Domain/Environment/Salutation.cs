using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Environment;

public class Salutation : BaseEntity<int>
{
    public int? TenantId { get; set; } // Für lokale auf den Mandanten angepasste Begrüßungen
    [Required]
    [StringLength(20)]
    public string Name { get; set; }
    [Required]
    [StringLength(40)]
    public string LetterGreeting { get; set; }
    [DefaultValue(1)]
    public int LanguageId { get; set; }
    [Required]
    [StringLength(40)]
    public string LetterClosing { get; set; }
    [Range(1, 900, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int? OrderNumber { get; set; }
}
