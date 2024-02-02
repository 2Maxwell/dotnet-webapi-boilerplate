using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Taxes;
public class TaxesByCountryIdSpec : Specification<Tax, TaxDto>
{
    public TaxesByCountryIdSpec(int countryId)
    {
        Query
            .Where(x => x.CountryId == countryId)
            .Include(x => x.TaxItems)
            .OrderBy(x => x.Id);
    }
}
