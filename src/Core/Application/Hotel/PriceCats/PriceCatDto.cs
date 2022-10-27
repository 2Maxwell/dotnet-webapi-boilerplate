namespace FSH.WebApi.Application.Hotel.PriceCats;

public class PriceCatDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int CategoryId { get; set; }
    public DateTime DatePrice { get; set; }
    public int Pax { get; set; }
    public decimal RateCurrent { get; set; }
    public decimal RateStart { get; set; }
    public decimal RateAutomatic { get; set; }
    public string? EventDates { get; set; } // Trenner , (Komma)
    public int RateTypeEnumId { get; set; } // Base, Season, Event, Fair
}
