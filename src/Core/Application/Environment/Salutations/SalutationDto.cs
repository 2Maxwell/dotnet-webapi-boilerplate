namespace FSH.WebApi.Application.Environment.Salutations;
public class SalutationDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; } // Für lokale auf den Mandanten angepasste Begrüßungen
    public string Name { get; set; }
    public string? LetterGreeting { get; set; }
    public int LanguageId { get; set; }
    public string? LetterClosing { get; set; }
    public int OrderNumber { get; set; }

}
