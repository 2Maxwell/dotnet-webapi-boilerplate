namespace FSH.WebApi.Application.Accounting.Mandants;
public class MandantSettingDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int ForecastDays { get; set; }
    public string? DefaultTransfer { get; set; }
    public DateTime DefaultArrivalTime { get; set; }
    public DateTime DefaultDepartureTime { get; set; }
    public int DefaultLanguageId { get; set; }
    public int DefaultCountryId { get; set; }
    public int DefaultGuestId { get; set; }
}
