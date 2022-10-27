using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.PluGroups;

public class PluGroupByNameSpec : Specification<PluGroup>, ISingleResultSpecification
{
    public PluGroupByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}
