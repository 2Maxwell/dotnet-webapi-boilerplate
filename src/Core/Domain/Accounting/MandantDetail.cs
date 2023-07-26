using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;
public class MandantDetail : AuditableEntity<int>, IAggregateRoot
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
    [StringLength(70)]
    public string? BankName { get; set; }
    [StringLength(22)]
    public string? IBAN { get; set; }
    [StringLength(11)]
    public string? BIC { get; set; }
    [StringLength(30)]
    public string? TaxId { get; set; }
    [StringLength(30)]
    public string? UStId { get; set; }
    [StringLength(100)]
    public string? Company { get; set; }
    [StringLength(30)]
    public string? CompanyRegister { get; set; }

    public MandantDetail(int mandantId, string? name1, string? name2, string? address1, string? address2, string? zip, string? city, int? countryId, int? stateRegionId, string? telephone, string? telefax, string? mobil, string? email, string? emailInvoice, string? webSite, int? languageId, string? bankName, string? iBAN, string? bIC, string? taxId, string? uStId, string? company, string? companyRegister)
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
        BankName = bankName;
        IBAN = iBAN;
        BIC = bIC;
        TaxId = taxId;
        UStId = uStId;
        Company = company;
        CompanyRegister = companyRegister;
    }

    public MandantDetail Update(string? name1, string? name2, string? address1, string? address2, string? zip, string? city, int? countryId, int? stateRegionId, string? telephone, string? telefax, string? mobil, string? email, string? emailInvoice, string? webSite, int? languageId, string? bankName, string? iBAN, string? bIC, string? taxId, string? uStId, string? company, string? companyRegister)
    {
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
        BankName = bankName;
        IBAN = iBAN;
        BIC = bIC;
        TaxId = taxId;
        UStId = uStId;
        Company = company;
        CompanyRegister = companyRegister;
        return this;
    }

}
