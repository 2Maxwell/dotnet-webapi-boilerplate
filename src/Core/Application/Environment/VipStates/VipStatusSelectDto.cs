using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.VipStates;
public class VipStatusSelectDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
}
