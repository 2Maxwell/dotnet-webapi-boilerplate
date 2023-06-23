namespace FSH.WebApi.Application.Accounting.Mandants;

public class MandantDto : IDto
{
    public int Id { get; set; }
    public string? Kz { get; set; }
    public string? Name { get; set; }
    public int GroupMember { get; set; }
    public bool GroupHead { get; set; }
    public DateTime? HotelDate { get; set; }
}
