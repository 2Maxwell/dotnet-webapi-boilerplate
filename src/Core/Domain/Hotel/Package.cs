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
    [StringLength(500)]
    public string? Display { get; set; }
    [Required]
    [StringLength(300)]
    public string? DisplayShort { get; set; }
    [Required]
    [StringLength(300)]
    public string? DisplayHighLight { get; set; }
    [Required]
    [StringLength(500)]
    public string? ConfirmationText { get; set; }
    [Required]
    [Range(1, 9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int InvoicePosition { get; set; } = 1;
    [StringLength(50)]
    public string? InvoiceName { get; set; }
    public int PackageBookingBaseEnumId { get; set; }
    public int PackageBookingRhythmEnumId { get; set; }
    [StringLength(200)]
    public string PackageTargetEnum { get; set; }
    public bool Optional { get; set; }
    public bool ShopExtern { get; set; }
    [StringLength(100)]
    public string? ChipIcon { get; set; }
    [StringLength(50)]
    public string? ChipText { get; set; }

    // PackageTypeEnum: SystemPackage(z.B. Logis buchen ist ein SystemPackage), ReservationPackage (nur innerhalb der Rate definierbar: Early, Weekend),
    // OptionPackage HotelOptionen, RestaurantOptionen, TagungsOptionen, WellnessOptionen, Kegelbahn...

    public Package(int mandantId, string? name, string? kz, string? description, string? display, string? displayShort, string? displayHighLight, string? confirmationText, int invoicePosition, string? invoiceName, int packageBookingBaseEnumId, int packageBookingRhythmEnumId, string packageTargetEnum, bool optional, bool shopExtern, string? chipIcon, string? chipText)
    {
        MandantId = mandantId;
        Name = name;
        Kz = kz;
        Description = description;
        Display = display;
        DisplayShort = displayShort;
        DisplayHighLight = displayHighLight;
        ConfirmationText = confirmationText;
        InvoicePosition = invoicePosition;
        InvoiceName = invoiceName;
        PackageBookingBaseEnumId = packageBookingBaseEnumId;
        PackageBookingRhythmEnumId = packageBookingRhythmEnumId;
        PackageTargetEnum = packageTargetEnum;
        Optional = optional;
        ShopExtern = shopExtern;
        ChipIcon = chipIcon;
        ChipText = chipText;
    }

    public Package Update(string? name, string? kz, string? description, string? display, string? displayShort, string? displayHighLight, string? confirmationText, int invoicePosition, string? invoiceName, int packageBookingBaseEnumId, int packageBookingRhythmEnumId, string packageTargetEnum, bool optional, bool shopExtern, string? chipIcon, string? chipText)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (kz is not null && Kz?.Equals(kz) is not true) Kz = kz;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (display is not null && Display?.Equals(display) is not true) Display = display;
        if (displayShort is not null && DisplayShort?.Equals(displayShort) is not true) DisplayShort = displayShort;
        if (displayHighLight is not null && DisplayHighLight?.Equals(displayHighLight) is not true) DisplayHighLight = displayHighLight;
        if (confirmationText is not null && confirmationText?.Equals(confirmationText) is not true) ConfirmationText = confirmationText;
        InvoicePosition = invoicePosition;
        if (invoiceName is not null && InvoiceName?.Equals(invoiceName) is not true) InvoiceName = invoiceName;
        PackageBookingBaseEnumId = packageBookingBaseEnumId;
        PackageBookingRhythmEnumId = packageBookingRhythmEnumId;
        PackageTargetEnum = packageTargetEnum;
        Optional = optional;
        ShopExtern = shopExtern;
        if (chipIcon is not null && ChipIcon?.Equals(chipIcon) is not true) ChipIcon = chipIcon;
        if (chipText is not null && ChipText?.Equals(chipText) is not true) ChipText = chipText;
        return this;
    }
}