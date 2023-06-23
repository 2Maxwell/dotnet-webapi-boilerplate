using FSH.WebApi.Application.Hotel.Categorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.ShopMandant;
public class CategoryRatesDto : IDto
{
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public int Adults { get; set; }
    public int Childs { get; set; }
    public string ChildsString { get; set; }
    public int BedsOccupied { get; set; }
    public string? PromotionCode { get; set; }
    public List<CategoryShopMandantDto>? categoryShopMandantDtos { get; set; }
}
