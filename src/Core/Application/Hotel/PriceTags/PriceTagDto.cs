using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PriceTags;
public class PriceTagDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int ReservationId { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public decimal AverageRate { get; set; }
    public decimal? UserRate { get; set; }
    public int RateSelected { get; set; } // 1 = Daily, 2 = AverageRate, 3 = UserRate
    public int CategoryId { get; set; }
    public List<PriceTagDetailDto>? PriceTagDetails { get; set; }
}
