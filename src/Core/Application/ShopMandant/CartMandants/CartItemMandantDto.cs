using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Application.Hotel.CancellationPolicys;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Shop;
using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Application.ShopMandant.CartMandants;
public class CartItemMandantDto : IDto
{
    public int CartItemSource { get; set; } // Reservation, Package, Wish, Message
    public decimal Amount { get; set; } = 1;
    public decimal Price { get; set; }
    public decimal PriceTotal
    {
        get
        {

            return (Amount * Price) + Convert.ToDecimal(PackageExtendedBookingLines.Sum(x => x.Total));
        }
    }

    public List<PriceCatDto>? PriceCats { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string? Name { get; set; }
    public int CategoryId { get; set; }

    public Pax Pax { get; set; }
    public int RateId { get; set; }
    public string? RatePackages { get; set; }
    public List<PersonShopItem> PersonList { get; set; }

    // public string? PersonsJsonString { get; set; }
    [StringLength(250)]
    public string? Wishes { get; set; }
    [StringLength(250)]
    public string? Remarks { get; set; }
    public string? ImagePath { get; set; }
    public List<PackageExtendDto>? PackageExtendedList { get; set; } //NOTE von PackageExtendedDto in PackageExtended geändert
    public List<BookingLineSummary>? PackageExtendedBookingLines { get; set; }
    public int BookingPolicyId { get; set; }
    public BookingPolicyDto BookingPolicy { get; set; } //muss in DTO Class oder partial Class im WASM
    public int CancellationPolicyId { get; set; }
    public CancellationPolicyDto CancellationPolicy { get; set; }  //muss in DTO Class oder partial Class im WASM

}
