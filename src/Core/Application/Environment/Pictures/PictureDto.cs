using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.Pictures;
public class PictureDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int OrderNumber { get; set; }
    public string? MatchCode { get; set; }
    public string? ImagePath { get; set; }
    public bool ShowPicture { get; set; } = true;
    public bool Publish { get; set; } = true;
    public bool DiaShow { get; set; } = true;
}
