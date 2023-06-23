using FSH.WebApi.Application.Accounting.Rates;
using FSH.WebApi.Application.Hotel.PriceCats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Categorys;
public class CategoryShopMandantDto : IDto
{
    public int Id { get; set; }
    public string? Kz { get; set; }
    public string? Name { get; set; }
    public int OrderNumber { get; set; }
    public string? Display { get; set; }
    public string? DisplayShort { get; set; }
    public string? DisplayHighLight { get; set; }
    public int NumberOfBeds { get; set; }
    public int NumberOfExtraBeds { get; set; }
    public string? ImagePath { get; set; }
    public int RoomsAvailable { get; set; }
    public List<RateShopMandantDto>? RatesList { get; set; } = new();
    public List<PriceCatDto>? PriceCatDtos { get; set; } = new(); 
}
