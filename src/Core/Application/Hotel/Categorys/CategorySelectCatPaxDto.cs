using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class CategorySelectCatPaxDto : IDto
{
    public string? Kz { get; set; }
    public int NumberOfBeds { get; set; }
    public int NumberOfExtraBeds { get; set; }
    public string? CatPax { get; set; }
}
