using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Hotel;

public class BookingPolicy : AuditableEntity<int>, IAggregateRoot
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
    public bool IsDefault { get; set; }
    public bool CreditCard { get; set; }
    public bool Deposit { get; set; }
    public int Priority { get; set; }
    [StringLength(100)]
    public string? ChipIcon { get; set; }
    [StringLength(50)]
    public string? ChipText { get; set; }

    public BookingPolicy(int mandantId, string? name, string? kz, string? description, string? display, string? displayShort, string? displayHighLight, string? confirmationText, bool isDefault, bool creditCard, bool deposit, int priority, string? chipIcon, string? chipText)
    {
        MandantId = mandantId;
        Name = name;
        Kz = kz;
        Description = description;
        Display = display;
        DisplayShort = displayShort;
        DisplayHighLight = displayHighLight;
        ConfirmationText = confirmationText;
        IsDefault = isDefault;
        CreditCard = creditCard;
        Deposit = deposit;
        Priority = priority;
        ChipIcon = chipIcon;
        ChipText = chipText;
    }

    public BookingPolicy Update(string? name, string? kz, string? description, string? display, string? displayShort, string? displayHighLight, string? confirmationText, bool isDefault, bool creditCard, bool deposit, int priority, string? chipIcon, string? chipText)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (kz is not null && Kz?.Equals(kz) is not true) Kz = kz;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (display is not null && Display?.Equals(display) is not true) Display = display;
        if (displayShort is not null && DisplayShort?.Equals(displayShort) is not true) DisplayShort = displayShort;
        if (displayHighLight is not null && DisplayHighLight?.Equals(displayHighLight) is not true) DisplayHighLight = displayHighLight;
        if (confirmationText is not null && confirmationText?.Equals(confirmationText) is not true) ConfirmationText = confirmationText;
        IsDefault = isDefault;
        CreditCard = creditCard;
        Deposit = deposit;
        Priority = priority;
        if (chipIcon is not null && ChipIcon?.Equals(chipIcon) is not true) ChipIcon = chipIcon;
        if (chipText is not null && ChipText?.Equals(chipText) is not true) ChipText = chipText;
        return this;
    }
}
