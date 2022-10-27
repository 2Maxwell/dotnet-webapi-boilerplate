using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Hotel;

public class CancellationPolicy : AuditableEntity<int>, IAggregateRoot
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
    [StringLength(150)]
    public string? DisplayShort { get; set; }
    [Required]
    [StringLength(500)]
    public string? Display { get; set; }
    [Required]
    [StringLength(500)]
    public string? ConfirmationText { get; set; }
    public bool IsDefault { get; set; }
    public int FreeCancellationDays { get; set; }
    public int Priority { get; set; }
    public int NoShow { get; set; }

    public CancellationPolicy(int mandantId, string? name, string? kz, string? description, string? displayShort, string? display, string? confirmationText, bool isDefault, int freeCancellationDays, int priority, int noShow)
    {
        MandantId = mandantId;
        Name = name;
        Kz = kz;
        Description = description;
        DisplayShort = displayShort;
        Display = display;
        ConfirmationText = confirmationText;
        IsDefault = isDefault;
        FreeCancellationDays = freeCancellationDays;
        Priority = priority;
        NoShow = noShow;
    }

    public CancellationPolicy Update(string? name, string? kz, string? description, string? displayShort, string? display, string? confirmationText, bool isDefault, int freeCancellationDays, int priority, int noShow)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (kz is not null && Kz?.Equals(kz) is not true) Kz = kz;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (displayShort is not null && DisplayShort?.Equals(displayShort) is not true) DisplayShort = displayShort;
        if (display is not null && Display?.Equals(display) is not true) Display = display;
        if (confirmationText is not null && confirmationText?.Equals(confirmationText) is not true) ConfirmationText = confirmationText;
        IsDefault = isDefault;
        FreeCancellationDays = freeCancellationDays;
        Priority = priority;
        NoShow = noShow;
        return this;
    }
}