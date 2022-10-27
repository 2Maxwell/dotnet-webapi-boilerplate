namespace FSH.WebApi.Application.Environment.Languages;

public class LanguageDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string StartTag { get; set; }
    public string EndTag { get; set; }
}
