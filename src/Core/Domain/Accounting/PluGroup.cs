using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;
// sollte MerchandiseGroup sein
public class PluGroup : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    [StringLength(30)]
    public string Name { get; set; }
    [Required]
    [Range(1, 200, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int OrderNumber { get; set; }

    public PluGroup(string name, int orderNumber)
    {
        Name = name;
        OrderNumber = orderNumber;
    }

    public PluGroup Update(string name, int orderNumber)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (orderNumber > 0 && OrderNumber != orderNumber) OrderNumber = orderNumber;
        return this;
    }

}


