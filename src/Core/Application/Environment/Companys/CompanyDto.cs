using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.Companys;
public class CompanyDto : IDto
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
    public int LanguageId { get; set; }
    public int CompanyTypEnumId { get; set; } // Company, TravelAgent, Portal
    public decimal? CommissionRate { get; set; }
    public bool AddTax { get; set; }
    public int GroupHeadId { get; set; } // Id of CompanyHead
    public int? PriceContractId { get; set; }
    public string? Kz { get; set; }
    public int? StatusId { get; set; }
    public string? Text { get; set; }
}
