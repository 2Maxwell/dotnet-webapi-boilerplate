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

// Header (PriceTag und PriceTagDetail) für PriceCats erstellen
// einstellen ob Tages, Misch oder Userpreis genommen wird.
// sollte bereits im ShopMandant möglich sein.
//
// ? ob RateStart und RateAutomatic für PriceTagDetail benötigt werden
// eventuell kann man die Ratenmaximierung ausrechnen wenn Preisautomatik
// in der Datei steht. Darüber ließe sich ermitteln wieviel die Preisautomatik anhand der
// Zimmeranzahl bringt.
