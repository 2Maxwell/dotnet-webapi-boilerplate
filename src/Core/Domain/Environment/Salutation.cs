using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Environment;

public class Salutation : AuditableEntity<int>, IAggregateRoot
{

    public int MandantId { get; set; } // Für lokale auf den Mandanten angepasste Begrüßungen
    [Required]
    [StringLength(30)]
    public string Name { get; set; }
    [Required]
    [StringLength(50)]
    public string? LetterGreeting { get; set; }
    [DefaultValue(1)]
    public int LanguageId { get; set; }
    [Required]
    [StringLength(50)]
    public string? LetterClosing { get; set; }
    [Range(1, 900, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int OrderNumber { get; set; }

    public Salutation(int mandantId, string name, string? letterGreeting, int languageId, string? letterClosing, int orderNumber)
    {
        MandantId = mandantId;
        Name = name;
        LetterGreeting = letterGreeting;
        LanguageId = languageId;
        LetterClosing = letterClosing;
        OrderNumber = orderNumber;
    }

    public Salutation Update(string name, string? letterGreeting, int languageId, string? letterClosing, int orderNumber)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (letterGreeting is not null && LetterGreeting?.Equals(letterGreeting) is not true) LetterGreeting = letterGreeting;
        LanguageId = languageId;
        if (letterClosing is not null && LetterClosing?.Equals(letterClosing) is not true) LetterClosing = letterClosing;
        OrderNumber = orderNumber;
        return this;
    }

}
