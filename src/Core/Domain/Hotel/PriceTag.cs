using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Hotel;

public class PriceTag : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    public int ReservationId { get; set; }
    [Required]
    public DateTime Arrival { get; set; }
    [Required]
    public DateTime Departure { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal AverageRate { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UserRate { get; set; }
    public int RateSelected { get; set; } // 1 = Daily, 2 = AverageRate, 3 = UserRate
    [Required]
    public int CategoryId { get; set; }
    public virtual List<PriceTagDetail> PriceTagDetails { get; set; }

    public PriceTag(int mandantId, int reservationId, DateTime arrival, DateTime departure, decimal averageRate, decimal? userRate, int rateSelected, int categoryId)
    {
        MandantId = mandantId;
        ReservationId = reservationId;
        Arrival = arrival;
        Departure = departure;
        AverageRate = averageRate;
        UserRate = userRate;
        RateSelected = rateSelected;
        CategoryId = categoryId;
    }

    public PriceTag Update(int reservationId, DateTime arrival, DateTime departure, decimal averageRate, decimal? userRate, int rateSelected, int categoryId)
    {
        ReservationId = reservationId;
        Arrival = arrival;
        Departure = departure;
        AverageRate = averageRate;
        UserRate = userRate;
        RateSelected = rateSelected;
        CategoryId = categoryId;
        return this;
    }

}