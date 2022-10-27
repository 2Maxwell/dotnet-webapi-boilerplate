using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Application.Hotel.CancellationPolicys;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Shop;

namespace FSH.WebApi.Application.Accounting.Rates;
public class RateShopMandantDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? DisplayShort { get; set; }
    public string? Display { get; set; }
    public int BookingPolicyId { get; set; }
    public int CancellationPolicyId { get; set; }
    public string? Packages { get; set; } // Value ist Packages.Kz mit , als Trenner
    public string? Categorys { get; set; } // Value ist Category mit , als Trenner
    public BookingPolicyDto? bookingPolicyDto { get; set; }
    public CancellationPolicyDto? cancellationPolicyDto { get; set; }
    public List<PackageDto> packagesList { get; set; } = new ();
    public List<BookingLine> bookingLinesList { get; set; } = new();

}
