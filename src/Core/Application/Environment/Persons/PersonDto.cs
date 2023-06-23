using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Persons;
public class PersonDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Source { get; set; } // GuestHotel, GuestRestaurant, CompanyContact, Sharer, MeetingContact
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? Title { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Zip { get; set; }
    public string? City { get; set; }
    public int CountryId { get; set; }
    public int StateRegionId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Telephone { get; set; }
    public string? Telefax { get; set; }
    public string? Mobil { get; set; }
    public string? Email { get; set; }
    public int? CompanyId { get; set; } = null;
    public int LanguageId { get; set; }
    public int NationalityId { get; set; }
    public bool PersonDelete { get; set; }
    public string? Wishes { get; set; }
    public string? RoomsPreferred { get; set; } // Trenner ist egal, können mehrere Zimmer genannt werden 
    public int SalutationId { get; set; }
    public string? Kz { get; set; } // siehe Source
    public int? StatusId { get; set; }
    public int? VipStatusId { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? Text { get; set; }
    public string SalutationName { get; set; }
}
