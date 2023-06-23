using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Environment;
public class Person : AuditableEntity<int>, IAggregateRoot
{
    //[Column("MandantId")]
    public int MandantId { get; set; }
    [StringLength(150)]
    public string? Source { get; set; } // GuestHotel, GuestRestaurant, CompanyContact, Sharer,
                                        // MeetingContact, BookerWithLogin, BookerWithOutLogin, Temporary,
                                        //

    [Required]
    [StringLength(100)]
    public string? Name { get; set; }
    [StringLength(50)]
    public string? FirstName { get; set; }
    [StringLength(50)]
    public string? Title { get; set; }
    [StringLength(100)]
    public string? Address1 { get; set; }
    [StringLength(100)]
    public string? Address2 { get; set; }
    [StringLength(12)]
    public string? Zip { get; set; }
    [StringLength(70)]
    public string? City { get; set; }
    public int? CountryId { get; set; }
    public int? StateRegionId { get; set; }
    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }
    //[Column("Telephone")]
    [StringLength(25)]
    public string? Telephone { get; set; }
    [StringLength(25)]
    public string? Telefax { get; set; }
    [StringLength(25)]
    public string? Mobil { get; set; }
    [StringLength(70)]
    public string? Email { get; set; }
    public int? CompanyId { get; set; } = null;
    public int? LanguageId { get; set; }
    public int? NationalityId { get; set; }
    public bool? PersonDelete { get; set; }
    [StringLength(250)]
    public string? Wishes { get; set; }
    [StringLength(50)]
    public string? RoomsPreferred { get; set; } // Trenner ist egal, können mehrere Zimmer genannt werden 
    public int SalutationId { get; set; }
    [StringLength(50)]
    public string? Kz { get; set; } // siehe Source
    public int? StatusId { get; set; }
    public int? VipStatusId { get; set; }
    [StringLength(50)]
    public string? Department { get; set; }
    [StringLength(50)]
    public string? Position { get; set; }
    [StringLength(150)]
    public string? Text { get; set; }
    [ForeignKey("SalutationId")]
    public virtual Salutation Salutation { get; set; } = default!;

    public Person(int mandantId, string? source, string? name, string? firstName, string? title, string? address1, string? address2, string? zip, string? city, int? countryId, int? stateRegionId, DateTime? dateOfBirth, string? telephone, string? telefax, string? mobil, string? email, int? companyId, int? languageId, int? nationalityId, bool? personDelete, string? wishes, string? roomsPreferred, int salutationId, string? kz, int? statusId, int? vipStatusId, string? department, string? position, string? text)
    {
        MandantId = mandantId;
        Source = source;
        Name = name;
        FirstName = firstName;
        Title = title;
        Address1 = address1;
        Address2 = address2;
        Zip = zip;
        City = city;
        CountryId = countryId;
        StateRegionId = stateRegionId;
        DateOfBirth = dateOfBirth;
        Telephone = telephone;
        Telefax = telefax;
        Mobil = mobil;
        Email = email;
        CompanyId = companyId;
        LanguageId = languageId;
        NationalityId = nationalityId;
        PersonDelete = personDelete;
        Wishes = wishes;
        RoomsPreferred = roomsPreferred;
        SalutationId = salutationId;
        Kz = kz;
        StatusId = statusId;
        VipStatusId = vipStatusId;
        Department = department;
        Position = position;
        Text = text;
    }

    // TODO Update anpassen an neue null Werte
    public Person Update(string? source, string? name, string? firstName, string? title, string? address1, string? address2, string? zip, string? city, int? countryId, int? stateRegionId, DateTime? dateOfBirth, string? telephone, string? telefax, string? mobil, string? email, int? companyId, int? languageId, int? nationalityId, bool? personDelete, string? wishes, string? roomsPreferred, int salutationId, string? kz, int? statusId, int? vipStatusId, string? department, string? position, string? text)
    {
        if (source is not null && Source?.Equals(source) is not true) Source = source;
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (firstName is not null && FirstName?.Equals(firstName) is not true) FirstName = firstName;
        if (title is not null && Title?.Equals(title) is not true) Title = title;
        if (address1 is not null && Address1?.Equals(address1) is not true) Address1 = address1;
        if (address2 is not null && Address2?.Equals(address2) is not true) Address2 = address2;
        if (zip is not null && Zip?.Equals(zip) is not true) Zip = zip;
        if (city is not null && City?.Equals(city) is not true) City = city;
        CountryId = countryId;
        StateRegionId = stateRegionId;
        if (dateOfBirth is not null && DateOfBirth?.Equals(dateOfBirth) is not true) DateOfBirth = dateOfBirth; DateOfBirth = dateOfBirth;
        if (telephone is not null && Telephone?.Equals(telephone) is not true) Telephone = telephone;
        if (telefax is not null && Telefax?.Equals(telefax) is not true) Telefax = telefax;
        if (mobil is not null && Mobil?.Equals(mobil) is not true) Mobil = mobil;
        if (email is not null && Email?.Equals(email) is not true) Email = email;
        CompanyId = companyId;
        LanguageId = languageId;
        NationalityId = nationalityId;
        PersonDelete = personDelete;
        if (wishes is not null && Wishes?.Equals(wishes) is not true) Wishes = wishes;
        if (roomsPreferred is not null && RoomsPreferred?.Equals(roomsPreferred) is not true) RoomsPreferred = roomsPreferred;
        SalutationId = salutationId;
        if (kz is not null && Kz?.Equals(kz) is not true) Kz = kz;
        StatusId = statusId;
        VipStatusId = vipStatusId;
        if (department is not null && Department?.Equals(department) is not true) Department = department;
        if (position is not null && Position?.Equals(position) is not true) Position = position;
        if (text is not null && Text?.Equals(text) is not true) Text = text;
        return this;
    }
}
