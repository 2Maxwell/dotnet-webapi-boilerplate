using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Helper;
[NotMapped]
public class ReportSupplements
{
    public string? ReportTitle { get; set; }
    public string? LogoPath { get; set; }
    public string? SearchString { get; set; }
}
