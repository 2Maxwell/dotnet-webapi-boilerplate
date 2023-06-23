using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Hotel;
public class PriceTagDetail : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int PriceTagId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public int RateId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime DatePrice { get; set; }
    [Required]
    [Range(1, 9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int PaxAmount { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal RateCurrent { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal RateStart { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal RateAutomatic { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UserRate { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal AverageRate { get; set; }
    [StringLength(50)]
    public string? EventDates { get; set; } // Trenner , (Komma)
    public int RateTypeEnumId { get; set; } // Base, Season, Event, Fair
    public bool NoShow { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? NoShowPercentage { get; set; }

    public PriceTagDetail(int priceTagId, int categoryId, int rateId, DateTime datePrice, int paxAmount, decimal rateCurrent, decimal rateStart, decimal rateAutomatic, decimal? userRate, decimal averageRate, string? eventDates, int rateTypeEnumId, bool noShow, decimal? noShowPercentage)
    {
        PriceTagId = priceTagId;
        CategoryId = categoryId;
        RateId = rateId;
        DatePrice = datePrice;
        PaxAmount = paxAmount;
        RateCurrent = rateCurrent;
        RateStart = rateStart;
        RateAutomatic = rateAutomatic;
        UserRate = userRate;
        AverageRate = averageRate;
        EventDates = eventDates;
        RateTypeEnumId = rateTypeEnumId;
        NoShow = noShow;
        NoShowPercentage = noShowPercentage;
    }

    public PriceTagDetail Update(decimal userRate, decimal averageRate, bool noShow, decimal? noShowPercentage)
    {
        UserRate = userRate;
        AverageRate = averageRate;
        NoShow = noShow;
        NoShowPercentage = noShowPercentage;
        return this;
    }

    public PriceTagDetail Delete(int priceTagId)
    {
        PriceTagId = priceTagId;
        return this;
    }
}
