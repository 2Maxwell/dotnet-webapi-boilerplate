using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting;

public class Item : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    [Range(1000, 99999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int ItemNumber { get; set; }
    public int TaxId { get; set; }
    public decimal Price { get; set; }
    public int ItemGroupId { get; set; }
    public bool Automatic { get; set; } = false;
    public virtual ItemGroup ItemGroup { get; set; }
    // public virtual Tax Tax { get; set; }

    // TODO InvoicePosition, AccountNumber


    public Item(int mandantId, string name, int itemNumber, int taxId, decimal price, int itemGroupId, bool automatic)
    {
        MandantId = mandantId;
        Name = name;
        ItemNumber = itemNumber;
        TaxId = taxId;
        Price = price;
        ItemGroupId = itemGroupId;
        Automatic = automatic;
    }

    public Item Update(string name, int itemNumber, int taxId, decimal price, int itemGroupId, bool automatic)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (itemNumber > 999 && ItemNumber != 0) ItemNumber = itemNumber;
        TaxId = taxId;
        Price = price;
        ItemGroupId = itemGroupId;
        Automatic = automatic;
        return this;
    }


}
