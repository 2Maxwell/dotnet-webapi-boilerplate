namespace FSH.WebApi.Application.Hotel.VCats;
public class VCatDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    public int Amount { get; set; }
    public int Available { get; set; }
    public int Sold { get; set; }
    public int Stay { get; set; }
    public int Blocked { get; set; }
    public int Arrival { get; set; }
    public int BedsInventory { get; set; }
    public int Beds { get; set; }
    public int ExtraBedsInventory { get; set; }
    public int ExtraBeds { get; set; }
    public int Adult { get; set; }
    public int Child { get; set; }
    public int Departure { get; set; }
    public int Breakfast { get; set; }
}