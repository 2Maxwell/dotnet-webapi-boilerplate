namespace FSH.WebApi.Domain.Shop;
public class ResQuery
{
    public string? DestinationCountry { get; set; }
    public string? DestinationTown { get; set; }
    public string? DestinationZipCode { get; set; }
    public string? DestinationDecimalCoordinates { get; set; }
    public int SearchPersonId { get; set; }
    public bool BookerIsGuest { get; set; }
    public int SearchCompanyId { get; set; }
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public string? PromotionCode { get; set; }
    public List<Pax>? RoomOccupancy { get; set; } = new List<Pax>();
    public int RoomAmount { get; set; }
    public int BedsTotal { get; set; }
}
