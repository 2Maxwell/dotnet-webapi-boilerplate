namespace FSH.WebApi.Application.Environment.Companys;

public class CompanyCommunicationDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Telephone { get; set; }
    public string? Telefax { get; set; }
    public string? Mobil { get; set; }
    public string? Email { get; set; }
}
