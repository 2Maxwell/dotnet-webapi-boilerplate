using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;
public class MandantSetting : AuditableEntity<int>, IAggregateRoot
{
    public int MandantId { get; set; }
    public int ForecastDays { get; set; }
    [StringLength(70)]
    public string? DefaultTransfer { get; set; }
    public DateTime DefaultArrivalTime { get; set; }
    public DateTime DefaultDepartureTime { get; set; }
    public int DefaultLanguageId { get; set; }
    public int DefaultCountryId { get; set; }
    public int DefaultGuestId { get; set; }

    public MandantSetting(int mandantId, int forecastDays, string? defaultTransfer, DateTime defaultArrivalTime, DateTime defaultDepartureTime, int defaultLanguageId, int defaultCountryId, int defaultGuestId)
    {
        MandantId = mandantId;
        ForecastDays = forecastDays;
        DefaultTransfer = defaultTransfer;
        DefaultArrivalTime = defaultArrivalTime;
        DefaultDepartureTime = defaultDepartureTime;
        DefaultLanguageId = defaultLanguageId;
        DefaultCountryId = defaultCountryId;
        DefaultGuestId = defaultGuestId;
    }

    public MandantSetting Update(int forecastDays, string? defaultTransfer, DateTime defaultArrivalTime, DateTime defaultDepartureTime, int defaultLanguageId, int defaultCountryId, int defaultGuestId)
    {
        ForecastDays = forecastDays;
        DefaultTransfer = defaultTransfer;
        DefaultArrivalTime = defaultArrivalTime;
        DefaultDepartureTime = defaultDepartureTime;
        DefaultLanguageId = defaultLanguageId;
        DefaultCountryId = defaultCountryId;
        DefaultGuestId = defaultGuestId;
        return this;
    }

}
