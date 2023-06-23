using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Enums;
public enum PackageTargetEnum
{
    HotelReservation = 100,
    HotelOption = 110,
    RestaurantReservation = 200,
    RestaurantOption = 210,
    Tagung = 300,
    TagungOption = 310,
    ShopHotel = 400,
    ShopRestaurant = 410,
    Other = 500,
    Cashier = 600,
    CashierHotel = 610,
    CashierRestaurant = 620,
    SystemPackage = 900
}
