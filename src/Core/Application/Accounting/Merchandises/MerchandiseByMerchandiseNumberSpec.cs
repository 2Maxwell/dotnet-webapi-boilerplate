using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Merchandises;

internal class MerchandiseByMerchandiseNumberSpec : Specification<Merchandise>, ISingleResultSpecification
{
    public MerchandiseByMerchandiseNumberSpec(int merchandiseNumber) =>
        Query.Where(p => p.MerchandiseNumber == merchandiseNumber);
}