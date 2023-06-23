using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
