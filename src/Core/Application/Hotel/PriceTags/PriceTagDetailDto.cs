using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.PriceTags;
public class PriceTagDetailDto
{
    public int Id { get; set; }
    public int PriceTagId { get; set; }
    public int CategoryId { get; set; }
    public int RateId { get; set; }
    public DateTime DatePrice { get; set; }
    public int PaxAmount { get; set; }
    public decimal RateCurrent { get; set; }
    public decimal RateStart { get; set; }
    public decimal RateAutomatic { get; set; }
    public decimal? UserRate { get; set; }
    public decimal AverageRate { get; set; }
    public string? EventDates { get; set; } // Trenner , (Komma)
    public int RateTypeEnumId { get; set; } // Base, Season, Event, Fair
    public bool NoShow { get; set; }
    public decimal? NoShowPercentage { get; set; }
}
