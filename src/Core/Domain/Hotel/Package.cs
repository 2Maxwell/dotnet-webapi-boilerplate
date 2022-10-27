using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Hotel;

public class Package : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(50)]
    public string? Name { get; set; }
    [Required]
    [StringLength(10)]
    public string? Kz { get; set; }
    [Required]
    [StringLength(200)]
    public string? Description { get; set; }
    [Required]
    [StringLength(50)]
    public string? Display { get; set; }
    [Required]
    [Range(1, 9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int InvoicePosition { get; set; } = 1;
    [StringLength(50)]
    public string? InvoiceName { get; set; }
    public int PackageBookingBaseEnumId { get; set; }
    public int PackageBookingRhythmEnumId { get; set; }
    public bool Optional { get; set; }
    public bool ShopExtern { get; set; }

    // public virtual PackageItem PackageItem { get; set; }

    public Package(int mandantId, string? name, string? kz, string? description, string? display,  int invoicePosition, string? invoiceName, int packageBookingBaseEnumId, int packageBookingRhythmEnumId, bool optional, bool shopExtern)
    {
        MandantId = mandantId;
        Name = name;
        Kz = kz;
        Description = description;
        Display = display;
        InvoicePosition = invoicePosition;
        InvoiceName = invoiceName;
        PackageBookingBaseEnumId = packageBookingBaseEnumId;
        PackageBookingRhythmEnumId = packageBookingRhythmEnumId;
        Optional = optional;
        ShopExtern = shopExtern;
    }

    public Package Update(string? name, string? kz, string? description, string? display, int invoicePosition, string? invoiceName, int packageBookingBaseEnumId, int packageBookingRhythmEnumId, bool optional, bool shopExtern)
    {
        Name = name;
        Kz = kz;
        Description = description;
        Display = display;
        InvoicePosition = invoicePosition;
        InvoiceName = invoiceName;
        PackageBookingBaseEnumId = packageBookingBaseEnumId;
        PackageBookingRhythmEnumId = packageBookingRhythmEnumId;
        Optional = optional;
        ShopExtern = shopExtern;
        return this;
    }
}