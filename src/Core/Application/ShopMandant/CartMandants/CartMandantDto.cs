using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Application.Hotel.CancellationPolicys;
using FSH.WebApi.Domain.Shop;

namespace FSH.WebApi.Application.ShopMandant.CartMandants;
public class CartMandantDto
{
    public Guid? CartId { get; set; }
    public int MandantId { get; set; }
    public int PersonId { get; set; }

    // geht nur bei Mandant im öffentlichen Shop kann nur
    // eine Person ausgewählt sein wenn ein Konto angelegt wurde.
    public bool BookerIsGuest { get; set; }
    public bool CreateGroupMaster { get; set; }
    public int CompanyId { get; set; }
    public int CompanyContactId { get; set; }
    public int TravelAgentId { get; set; }
    public int TravelAgentContactId { get; set; }

    // nur ShopMandant
    public int ShopPaymentId { get; set; }
    public DateTime HoldCartTill { get; set; }
    public string? MatchCode { get; set; }
    public List<CartItemMandantDto>? CartItemList { get; set; }

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
    public BookingPolicyDto? BookingPolicyDto { get; set; }
    public CancellationPolicyDto? CancellationPolicyDto { get; set; }
}
