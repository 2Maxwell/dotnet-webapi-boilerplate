using FSH.WebApi.Domain.Accounting;
using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Hotel;

public class PriceCat : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    // [Required]

    // public int RateId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime DatePrice { get; set; }
    [Required]
    [Range(1, 9, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Pax { get; set; }
    public decimal RateCurrent { get; set; }
    public decimal RateStart { get; set; }
    public decimal RateAutomatic { get; set; }

    [StringLength(50)]
    public string? EventDates { get; set; } // Trenner , (Komma)
    public int RateTypeEnumId { get; set; } // Base, Season, Event, Fair
    //public int RateScopeEnumId { get; set; } // public, private
    public virtual Category Category { get; private set; } = default!;


    public PriceCat(int mandantId, int categoryId, DateTime datePrice, int pax, decimal rateCurrent, decimal rateStart, decimal rateAutomatic, string? eventDates, int rateTypeEnumId)
    {
        MandantId = mandantId;

        // RateId = rateId;
        CategoryId = categoryId;
        DatePrice = datePrice;
        Pax = pax;
        RateCurrent = rateCurrent;
        RateStart = rateStart;
        RateAutomatic = rateAutomatic;
        EventDates = eventDates;
        RateTypeEnumId = rateTypeEnumId;
    }

    public PriceCat Update(decimal rateCurrent, decimal rateStart, decimal rateAutomatic, string? eventDates, int rateTypeEnumId)
    {
        RateCurrent = rateCurrent;
        RateStart = rateStart;
        RateAutomatic = rateAutomatic;
        if (eventDates is not null && EventDates?.Equals(eventDates) is not true) EventDates = eventDates;
        RateTypeEnumId = rateTypeEnumId;
        return this;
    }

    public PriceCat UpdateSinglePrice(decimal rateCurrent, decimal rateStart)
    {
        RateCurrent = rateCurrent;
        RateStart = rateStart;
        return this;
    }

}