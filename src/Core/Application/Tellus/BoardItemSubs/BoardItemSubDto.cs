using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Tellus.BoardItemSubs;
public class BoardItemSubDto : IDto
{

    public int Id { get; set; }
    public int MandantId { get; set; }
    public int BoardItemId { get; set; }
    public int OrderNumber { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
    public string? ResultType { get; set; }
    public string? ResultLabel { get; set; }
    public int? ResultValueInt { get; set; }
    public decimal? ResultValueDecimal { get; set; }
    public string? ResultValueString { get; set; }
    public bool? ResultValueBool { get; set; }
}
