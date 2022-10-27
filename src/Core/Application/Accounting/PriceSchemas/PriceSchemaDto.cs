using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.PriceSchemas;

public class PriceSchemaDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int RateTypeEnumId { get; set; }
    public string? BaseCatPax { get; set; }
    public List<PriceSchemaDetailDto> PriceSchemaDetails { get; set; } = new();
}
