using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Accounting;

public class Merchandise : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    [Range(1000, 99999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int MerchandiseNumber { get; set; }
    public int TaxId { get; set; }
    public decimal Price { get; set; }
    public int MerchandiseGroupId { get; set; }
    public bool Automatic { get; set; } = false;

    public Merchandise(string name, int merchandiseNumber, int taxId, decimal price, int merchandiseGroupId, bool automatic)
    {
        Name = name;
        MerchandiseNumber = merchandiseNumber;
        TaxId = taxId;
        Price = price;
        MerchandiseGroupId = merchandiseGroupId;
        Automatic = automatic;
    }

    public Merchandise Update(string name, int merchandiseNumber, int taxId, decimal price, int merchandiseGroupId, bool automatic)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (merchandiseNumber > 999 && MerchandiseNumber != 0) MerchandiseNumber = merchandiseNumber;
        TaxId = taxId;
        Price = price;
        MerchandiseGroupId = merchandiseGroupId;
        Automatic = automatic;
        return this;
    }

}
