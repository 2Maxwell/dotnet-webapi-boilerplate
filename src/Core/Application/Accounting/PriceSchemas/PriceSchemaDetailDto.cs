using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.PriceSchemas;

public class PriceSchemaDetailDto : IDto
{
    public int Id { get; set; }
    public int PriceSchemaId { get; set; }
    public string? TargetCatPax { get; set; }
    public decimal TargetDifference { get; set; }

}
