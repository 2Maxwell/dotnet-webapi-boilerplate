using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Taxes;

public class TaxDtoByTaxCountryIdSpec : Specification<Tax, TaxDto>
{
    public TaxDtoByTaxCountryIdSpec(int taxCountryId, int mandantId)
    {
        Query.Where(c => (c.CountryId == taxCountryId && c.MandantId == 0) || c.MandantId == mandantId);
    }
}
