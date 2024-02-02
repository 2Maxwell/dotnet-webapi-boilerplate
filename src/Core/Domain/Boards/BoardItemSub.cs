using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Boards;

public class BoardItemSub : AuditableEntity<int>, IAggregateRoot
{
    public BoardItemSub(int mandantId, int boardItemId, int orderNumber, string? title, string? text, string? resultType, string? resultLabel, int? resultValueInt, decimal? resultValueDecimal, string? resultValueString, bool? resultValueBool)
    {
        MandantId = mandantId;
        BoardItemId = boardItemId;
        OrderNumber = orderNumber;
        Title = title;
        Text = text;
        ResultType = resultType;
        ResultLabel = resultLabel;
        ResultValueInt = resultValueInt;
        ResultValueDecimal = resultValueDecimal;
        ResultValueString = resultValueString;
        ResultValueBool = resultValueBool;
    }

    [Required]
    public int MandantId { get; set; }
    [Required]
    public int BoardItemId { get; set; }
    [Required]
    public int OrderNumber { get; set; }
    [Required]
    [StringLength(50)]
    public string? Title { get; set; }
    [StringLength(250)]
    public string? Text { get; set; }
    [StringLength(20)]
    public string? ResultType { get; set; } // int, decimal, string, bool - Auswahl darstellen als RadioGroup
    [StringLength(40)]
    public string? ResultLabel { get; set; } // Obsolete
    public int? ResultValueInt { get; set; }
    public decimal? ResultValueDecimal { get; set; }
    [StringLength(250)]
    public string? ResultValueString { get; set; }
    public bool? ResultValueBool { get; set; }

    public BoardItemSub Update(int orderNumber, string? title, string? text, string? resultType, string? resultLabel, int? resultValueInt, decimal? resultValueDecimal, string? resultValueString, bool? resultValueBool)
    {
        if (OrderNumber != orderNumber) OrderNumber = orderNumber;
        if (title is not null && Title?.Equals(title) is not true) Title = title;
        if (text is not null && Text?.Equals(text) is not true) Text = text;
        ResultType = resultType;
        ResultLabel = resultLabel;
        ResultValueInt = resultValueInt;
        ResultValueDecimal = resultValueDecimal;
        ResultValueString = resultValueString;
        ResultValueBool = resultValueBool;
        return this;
    }
}
