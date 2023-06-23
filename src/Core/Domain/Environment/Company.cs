using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Environment;

public class Company : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(100)]
    public string? Name1 { get; set; }
    [StringLength(100)]
    public string? Name2 { get; set; }
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
    [StringLength(25)]
    public string? Telephone { get; set; }
    [StringLength(25)]
    public string? Telefax { get; set; }
    [StringLength(25)]
    public string? Mobil { get; set; }
    [StringLength(70)]
    public string? Email { get; set; }
    [StringLength(70)]
    public string? EmailInvoice { get; set; }
    [StringLength(70)]
    public string? WebSite { get; set; }
    public int? LanguageId { get; set; }
    public int? CompanyTypEnumId { get; set; } // Company, TravelAgent, Portal
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CommissionRate { get; set; }
    public bool? AddTax { get; set; }
    public int? GroupHeadId { get; set; } // Id of CompanyHead
    public int? PriceContractId { get; set; }
    [StringLength(50)]
    public string? Kz { get; set; }
    public int? StatusId { get; set; }
    [StringLength(150)]
    public string? Text { get; set; }

    public Company(int mandantId, string? name1, string? name2, string? address1, string? address2, string? zip, string? city, int? countryId, int? stateRegionId, string? telephone, string? telefax, string? mobil, string? email, string? emailInvoice, string? webSite, int? languageId, int? companyTypEnumId, decimal? commissionRate, bool? addTax, int? groupHeadId, int? priceContractId, string? kz, int? statusId, string? text)
    {
        MandantId = mandantId;
        Name1 = name1;
        Name2 = name2;
        Address1 = address1;
        Address2 = address2;
        Zip = zip;
        City = city;
        CountryId = countryId;
        StateRegionId = stateRegionId;
        Telephone = telephone;
        Telefax = telefax;
        Mobil = mobil;
        Email = email;
        EmailInvoice = emailInvoice;
        WebSite = webSite;
        LanguageId = languageId;
        CompanyTypEnumId = companyTypEnumId;
        CommissionRate = commissionRate;
        AddTax = addTax;
        GroupHeadId = groupHeadId;
        PriceContractId = priceContractId;
        Kz = kz;
        StatusId = statusId;
        Text = text;
    }

    public Company Update(string? name1, string? name2, string? address1, string? address2, string? zip, string? city, int? countryId, int? stateRegionId, string? telephone, string? telefax, string? mobil, string? email, string? emailInvoice, string? webSite, int? languageId, int? companyTypEnumId, decimal? commissionRate, bool? addTax, int? groupHeadId, int? priceContractId, string? kz, int? statusId, string? text)
    {
        if (name1 is not null && Name1?.Equals(name1) is not true) Name1 = name1;
        if (name2 is not null && Name2?.Equals(name2) is not true) Name2 = name2;
        if (address1 is not null && Address1?.Equals(address1) is not true) Address1 = address1;
        if (address2 is not null && Address2?.Equals(address2) is not true) Address2 = address2;
        if (zip is not null && Zip?.Equals(zip) is not true) Zip = zip;
        if (city is not null && City?.Equals(city) is not true) City = city;
        CountryId = countryId;
        StateRegionId = stateRegionId;
        if (telephone is not null && Telephone?.Equals(telephone) is not true) Telephone = telephone;
        if (telefax is not null && Telefax?.Equals(telefax) is not true) Telefax = telefax;
        if (mobil is not null && Mobil?.Equals(mobil) is not true) Mobil = mobil;
        if (email is not null && Email?.Equals(email) is not true) Email = email;
        if (emailInvoice is not null && EmailInvoice?.Equals(emailInvoice) is not true) EmailInvoice = emailInvoice;
        if (webSite is not null && WebSite?.Equals(webSite) is not true) WebSite = webSite;
        LanguageId = languageId;
        CompanyTypEnumId = companyTypEnumId;
        CommissionRate = commissionRate;
        AddTax = addTax;
        GroupHeadId = groupHeadId;
        PriceContractId = priceContractId;
        if (kz is not null && Kz?.Equals(kz) is not true) Kz = kz;
        StatusId = statusId;
        if (text is not null && Text?.Equals(text) is not true) Text = text;
        return this;
    }

}

// TODO Erstellen einer Rangliste der Firmen TOP 100 TOP 50 TOP 10 aufgrund der Umsätze der Firmen
// beachten das die Daten aktuell sein müssen und nicht aufgrund von alten Umsätzen erstellt werden.
// aktuelles Jahr, Vorjahr, Jahr davor