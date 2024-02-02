using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Taxes;

[Obsolete]
public class TaxSelectByMandantIdSpec : Specification<Tax, TaxSelectDto>
{
    public TaxSelectByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId);
    }
}
