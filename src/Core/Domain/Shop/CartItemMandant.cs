using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Shop;
public class CartItemMandant
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

    public List<PriceCat>? PriceCats { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string? Name { get; set; }
    public int CategoryId { get; set; }
    public Pax Pax { get; set; }
    public int RateId { get; set; }
    public List<PersonShopItem> PersonList { get; set; }

    // public string? PersonsJsonString { get; set; }
    [StringLength(250)]
    public string? Wishes { get; set; }
    [StringLength(250)]
    public string? Remarks { get; set; }
    public string? ImagePath { get; set; }
    public List<PackageExtend>? PackageExtendedList { get; set; } //NOTE von PackageExtendedDto in PackageExtended geändert
    public List<BookingLineSummary>? PackageExtendedBookingLines { get; set; }
    public int BookingPolicyId { get; set; }
    // public BookingPolicyDto BookingPolicy { get; set; } muss in DTO Class oder partial Class im WASM
    public int CancellationPolicyId { get; set; }
    // public CancellationPolicyDto CancellationPolicy { get; set; }  muss in DTO Class oder partial Class im WASM

}

public class PersonShopItem
{
    public int PersonId { get; set; }
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string PersonShopType { get; set; }
    public int ChildAge { get; set; }
    public bool ExtraBed { get; set; }
    public string? Index { get; set; }
}

public class CartMandant
{
    public Guid? CartId { get; set; }
    public int MandantId { get; set; }
    public int PersonId { get; set; }

    // geht nur bei Mandant im öffentlichen Shop kann nur
    // eine Person ausgewählt sein wenn ein Konto angelegt wurde.
    public bool BookerIsGuest { get; set; }
    public bool CreateGroupMaster{ get; set; }
    public int CompanyId { get; set; }
    public int CompanyContactId { get; set; }
    public int TravelAgentId { get; set; }
    public int TravelAgentContactId { get; set; }

    // nur ShopMandant
    public int ShopPaymentId { get; set; }
    public DateTime HoldCartTill { get; set; }
    public string? MatchCode { get; set; }
    public List<CartItemMandant>? CartItemList { get; set; }

    public int CartItemCount
    {
        get
        {
            int result = 0;
            if (CartItemList != null)
            {
                result = CartItemList.Count;
            }

            return result;
        }
    }

    public int CartItemCountAmount
    {
        get
        {
            int result = 0;
            if (CartItemList != null)
            {
                foreach (var item in CartItemList)
                {
                    result += Convert.ToInt16(item.Amount);
                }
            }

            return result;
        }
    }

    public decimal? CartPrice
    {
        get
        {
            decimal result = 0;
            if (CartItemList != null)
            {
                foreach (var item in CartItemList)
                {
                    result += item.PriceTotal;
                }
            }

            return result;

        }
    }

    public int BookingPolicyId { get; set; }
    public int CancellationPolicyId { get; set; }
    public string? Confirmations { get; set; }

    //public BookingPolicyDto ValidBookingPolicy
    //{
    //    get
    //    {
    //        BookingPolicyDto result = null;
    //        if (CartItemList == null) return result;
    //        if (CartItemList.Count == 1)
    //        {
    //            result = CartItemList[0].BookingPolicy;
    //        }
    //        else
    //        {
    //            int prio = 0;
    //            foreach (CartItemMandant cartItemMandant in CartItemList)
    //            {
    //                if (cartItemMandant.BookingPolicy.Priority > prio)
    //                {
    //                    result = cartItemMandant.BookingPolicy;
    //                }
    //            }
    //        }

    //        return result;
    //    }
    //}

    //public CancellationPolicyDto ValidCancellationPolicy
    //{
    //    get
    //    {
    //        CancellationPolicyDto result = null;
    //        if (CartItemList == null) return result;
    //        if (CartItemList.Count == 1)
    //        {
    //            result = CartItemList[0].CancellationPolicy;
    //        }
    //        else
    //        {
    //            int prio = 0;
    //            foreach (CartItemMandant cartItemMandant in CartItemList)
    //            {
    //                if (cartItemMandant.CancellationPolicy.Priority > prio)
    //                {
    //                    result = cartItemMandant.CancellationPolicy;
    //                }
    //            }
    //        }

    //        return result;
    //    }
    //}

}