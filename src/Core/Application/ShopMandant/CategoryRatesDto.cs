using FSH.WebApi.Application.Hotel.Categorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.ShopMandant;
public class CategoryRatesDto : IDto
{
    public List<CategoryShopMandantDto>? categoryShopMandantDtos { get; set; }
}
