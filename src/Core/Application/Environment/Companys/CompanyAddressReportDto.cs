namespace FSH.WebApi.Application.Environment.Companys;
public class CompanyAddressReportDto : IDto
{
    public string? Name1 { get; set; }
    public string? Name2 { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Zip { get; set; }
    public string? City { get; set; }
    public int? CountryId { get; set; }
    public int? StateRegionId { get; set; }
}
