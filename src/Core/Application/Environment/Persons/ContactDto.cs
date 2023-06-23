namespace FSH.WebApi.Application.Environment.Persons;
public class ContactDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Source { get; set; } // GuestHotel, GuestRestaurant, CompanyContact, Sharer, MeetingContact
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? Title { get; set; }
    public int? CompanyId { get; set; } = null;
    public int LanguageId { get; set; }
    public int SalutationId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Telephone { get; set; }
    public string? Telefax { get; set; }
    public string? Mobil { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
}
