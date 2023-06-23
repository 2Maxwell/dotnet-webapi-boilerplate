namespace FSH.WebApi.Application.Environment.Companys;
public class CompanySearchDto : IDto
{
    public int Id { get; set; }
    public string? Name1 { get; set; }
    public string? Name2 { get; set; }
    public string? Address1 { get; set; }
    public string? Zip { get; set; }
    public string? City { get; set; }
}
