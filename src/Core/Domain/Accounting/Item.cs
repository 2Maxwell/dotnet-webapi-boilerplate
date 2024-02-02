using System.ComponentModel.DataAnnotations;

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
    // public int TaxId { get; set; }
    // public decimal Price { get; set; }
    public int ItemGroupId { get; set; }
    public bool Automatic { get; set; } = false;
    public int InvoicePosition { get; set; }
    public int AccountNumber { get; set; }
    public virtual ItemGroup ItemGroup { get; set; }

    public Item(int mandantId, string name, int itemNumber, int itemGroupId, bool automatic, int invoicePosition, int accountNumber)
    {
        MandantId = mandantId;
        Name = name;
        ItemNumber = itemNumber;
        ItemGroupId = itemGroupId;
        Automatic = automatic;
        InvoicePosition = invoicePosition;
        AccountNumber = accountNumber;
    }

    public Item Update(string name, int itemNumber, int itemGroupId, bool automatic, int invoicePosition, int accountNumber)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (itemNumber > 999 && ItemNumber != 0) ItemNumber = itemNumber;
        ItemGroupId = itemGroupId;
        Automatic = automatic;
        InvoicePosition = invoicePosition;
        AccountNumber = accountNumber;
        return this;
    }
}
