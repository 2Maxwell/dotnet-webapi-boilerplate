using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Taxes;

public class TaxSelectByTaxCountryIdSpec : Specification<Tax, TaxSelectDto>
{
    public TaxSelectByTaxCountryIdSpec(int taxCountryId, int mandantId)
    {
        Query.Where(c => (c.CountryId == taxCountryId && c.MandantId == 0) || c.MandantId == mandantId);
    }
}
