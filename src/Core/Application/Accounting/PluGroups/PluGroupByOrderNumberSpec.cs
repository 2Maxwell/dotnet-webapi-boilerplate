using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.PluGroups;

public class PluGroupByOrderNumberSpec : Specification<PluGroup>, ISingleResultSpecification
{
    public PluGroupByOrderNumberSpec(int orderNumber) =>
        Query.Where(p => p.OrderNumber == orderNumber);
}
