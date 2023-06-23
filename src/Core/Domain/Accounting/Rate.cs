using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;

public class Rate : AuditableEntity<int>, IAggregateRoot
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
    [StringLength(300)]
    public string? DisplayShort { get; set; }
    [Required]
    [StringLength(500)]
    public string? Display { get; set; }
    public int BookingPolicyId { get; set; }
    public int CancellationPolicyId { get; set; }
    [StringLength(200)]
    public string? Packages { get; set; } // Value ist Packages.Kz mit , als Trenner
    [StringLength(200)]
    public string? Categorys { get; set; } // Value ist Category mit , als Trenner
    public bool RuleFlex { get; set; }
    public int RateTypeEnumId { get; set; } // Base, Season, Event, Fair
    public int RateScopeEnumId { get; set; } // Public, Private
    [StringLength(300)]
    public string? DisplayHighLight { get; set; }
    [StringLength(500)]
    public string? ConfirmationText { get; set; }
    [StringLength(100)]
    public string? ChipIcon { get; set; }
    [StringLength(50)]
    public string? ChipText { get; set; }

    public Rate(int mandantId, string? name, string? kz, string? description, string? displayShort, string? display, int bookingPolicyId, int cancellationPolicyId, string? packages, string? categorys, bool ruleFlex, int rateTypeEnumId, int rateScopeEnumId, string? displayHighLight, string? confirmationText, string? chipIcon, string? chipText)
    {
        MandantId = mandantId;
        Name = name;
        Kz = kz;
        Description = description;
        DisplayShort = displayShort;
        Display = display;
        BookingPolicyId = bookingPolicyId;
        CancellationPolicyId = cancellationPolicyId;
        Packages = packages;
        Categorys = categorys;
        RuleFlex = ruleFlex;
        RateTypeEnumId = rateTypeEnumId;
        RateScopeEnumId = rateScopeEnumId;
        DisplayHighLight = displayHighLight;
        ConfirmationText = confirmationText;
        ChipIcon = chipIcon;
        ChipText = chipText;
    }

    public Rate Update(string? name, string? kz, string? description, string? displayShort, string? display, int bookingPolicyId, int cancellationPolicyId, string? packages, string? categorys, bool ruleFlex, int rateTypeEnumId, int rateScopeEnumId, string? displayHighLight, string? confirmationText, string? chipIcon, string? chipText)
    {
        // MandantId = mandantId;
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (kz is not null && Kz?.Equals(kz) is not true) Kz = kz;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (displayShort is not null && DisplayShort?.Equals(displayShort) is not true) DisplayShort = displayShort;
        if (display is not null && Display?.Equals(display) is not true) Display = display;
        BookingPolicyId = bookingPolicyId;
        CancellationPolicyId = cancellationPolicyId;
        Packages = packages;
        Categorys = categorys;
        RuleFlex = ruleFlex;
        RateTypeEnumId = rateTypeEnumId;
        RateScopeEnumId = rateScopeEnumId;
        if (displayHighLight is not null && DisplayHighLight?.Equals(displayHighLight) is not true) DisplayHighLight = displayHighLight;
        if (confirmationText is not null && confirmationText?.Equals(confirmationText) is not true) ConfirmationText = confirmationText;
        if (chipIcon is not null && ChipIcon?.Equals(chipIcon) is not true) ChipIcon = chipIcon;
        if (chipText is not null && ChipText?.Equals(chipText) is not true) ChipText = chipText;
        return this;
    }
}
