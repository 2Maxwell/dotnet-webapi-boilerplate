using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Merchandises;

public class MerchandiseByNameSpec : Specification<Merchandise>, ISingleResultSpecification
{
    public MerchandiseByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}