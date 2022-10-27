namespace FSH.WebApi.Application.Environment.Languages;

public class LanguageByNameSpec : Specification<FSH.WebApi.Domain.Environment.Language>, ISingleResultSpecification
{
    public LanguageByNameSpec(string name) =>
        Query.Where(l => l.Name == name);
}
