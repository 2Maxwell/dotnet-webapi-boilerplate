using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.VipStates;
public class VipStatusDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Arrival { get; set; }
    public string? Daily { get; set; }
    public string? Departure { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
}
