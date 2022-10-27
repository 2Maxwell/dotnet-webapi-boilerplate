using FSH.WebApi.Application.Hotel.Periods;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Packages;

public class PackageDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? Display { get; set; }
    public int InvoicePosition { get; set; } = 1;
    public string? InvoiceName { get; set; }
    public int PackageBookingBaseEnumId { get; set; }
    public int PackageBookingRhythmEnumId { get; set; }
    public bool Optional { get; set; }
    public bool ShopExtern { get; set; }
    public List<PackageItemDto>? PackageItems { get; set; } = new();

}
