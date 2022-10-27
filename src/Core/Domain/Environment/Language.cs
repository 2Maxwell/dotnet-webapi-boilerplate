using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Environment;

public class Language : BaseEntity<int>, IAggregateRoot
{

    [Required]
    [StringLength(20)]
    public string Name { get; set; }
    [Required]
    [StringLength(10)]
    public string StartTag { get; set; }
    [Required]
    [StringLength(10)]
    public string EndTag { get; set; }
    [StringLength(10)]
    public string LanguageCode { get; set; }

    public Language(string name, string startTag, string endTag, string languageCode)
    {
        Name = name;
        StartTag = startTag;
        EndTag = endTag;
        LanguageCode = languageCode;
    }

    public Language Update(string name, string startTag, string endTag)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (startTag is not null && StartTag?.Equals(startTag) is not true) StartTag = startTag;
        if (endTag is not null && EndTag?.Equals(EndTag) is not true) EndTag = endTag;
        return this;
    }

    public string GetLanguageValueByLanguage(string input)
    {
        if (!input.Contains("|<")) // kein LanguageTag vorhanden
        {
            return input;
        }

        if (!input.Contains(EndTag)) // wird automatisch eng zurückgeliefert
        {
            string start = "|<eng|";
            string ende = "|eng>|";
            int posStart = input.IndexOf(start);
            int posEnde = input.IndexOf(ende);
            string result = input.Substring(posStart + start.Length, posEnde - (posStart + start.Length));
            return result;
        }
        else // wird die entsprechende Sprache zurückgeliefert
        {
            string start = StartTag;
            string ende = EndTag;
            int posStart = input.IndexOf(start);
            int posEnde = input.IndexOf(ende);
            string result = input.Substring(posStart + start.Length, posEnde - (posStart + start.Length));
            return result;
        }
    }
}
