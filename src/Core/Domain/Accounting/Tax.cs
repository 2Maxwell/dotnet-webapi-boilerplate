using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Accounting;
public class Tax : AuditableEntity<int>, IAggregateRoot
{

    public int MandantId { get; set; }
    public int CountryId { get; set; }
    [StringLength(50)]
    public string? Name { get; set; }
    [StringLength(50)]
    public int TaxSystemEnumId { get; set; }
    public virtual List<TaxItem>? TaxItems { get; set; }

    public Tax(int mandantId, int countryId, string? name, int taxSystemEnumId)
    {
        MandantId = mandantId;
        CountryId = countryId;
        Name = name;
        TaxSystemEnumId = taxSystemEnumId;
    }

    public Tax Update(int countryId, string? name, int taxSystemEnumId)
    {
        CountryId = countryId;
        Name = name;
        TaxSystemEnumId = taxSystemEnumId;
        return this;
    }

}
