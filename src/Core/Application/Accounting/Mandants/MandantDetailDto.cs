namespace FSH.WebApi.Application.Accounting.Mandants;
public class MandantDetailDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name1 { get; set; }
    public string? Name2 { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Zip { get; set; }
    public string? City { get; set; }
    public int? CountryId { get; set; }
    public int? StateRegionId { get; set; }
    public string? Telephone { get; set; }
    public string? Telefax { get; set; }
    public string? Mobil { get; set; }
    public string? Email { get; set; }
    public string? EmailInvoice { get; set; }
    public string? WebSite { get; set; }
    public int? LanguageId { get; set; }
    public string? BankName { get; set; }
    public string? IBAN { get; set; }
    public string? BIC { get; set; }
    public string? TaxId { get; set; }
    public string? UStId { get; set; }
    public string? Company { get; set; }
    public string? CompanyRegister { get; set; }
}
