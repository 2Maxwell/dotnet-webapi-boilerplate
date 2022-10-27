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
    [StringLength(150)]
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

    public Rate(int mandantId, string? name, string? kz, string? description, string? displayShort, string? display, int bookingPolicyId, int cancellationPolicyId, string? packages, string? categorys, bool ruleFlex, int rateTypeEnumId, int rateScopeEnumId)
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
    }

    public Rate Update(string? name, string? kz, string? description, string? displayShort, string? display, int bookingPolicyId, int cancellationPolicyId, string? packages, string? categorys, bool ruleFlex, int rateTypeEnumId, int rateScopeEnumId)
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
        return this;
    }
}
