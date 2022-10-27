using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting;

public class ItemGroup : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(30)]
    public string Name { get; set; }
    [Required]
    [Range(1, 999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int OrderNumber { get; set; }

    public ItemGroup(int mandantId, string name, int orderNumber)
    {
        MandantId = mandantId;
        Name = name;
        OrderNumber = orderNumber;
    }

    public ItemGroup Update(string name, int orderNumber)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (orderNumber > 0 && OrderNumber != orderNumber) OrderNumber = orderNumber;
        return this;
    }

}
