using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Boards;

public class BoardItemLabel : AuditableEntity<int>, IAggregateRoot
{
    public BoardItemLabel(int mandantId, string? text, string? color, bool defaultLabel, int? boardItemLabelGroupId)
    {
        MandantId = mandantId;
        Text = text;
        Color = color;
        DefaultLabel = defaultLabel;
        BoardItemLabelGroupId = boardItemLabelGroupId;
    }

    [Required]
    public int MandantId { get; set; }
    [StringLength(20)]
    public string? Text { get; set; }
    [Required]
    [StringLength(25)]
    public string? Color { get; set; }
    public bool DefaultLabel { get; set; } // wird in jeder BoardsCollection angezeigt
    [Required]
    public int? BoardItemLabelGroupId { get; set; }

    public BoardItemLabel Update(string? text, string? color, bool defaultLabel)
    {
        if (text is not null && Text?.Equals(text) is not true) Text = text;
        if (color is not null && Color?.Equals(color) is not true) Color = Color;
        DefaultLabel = defaultLabel;
        if (BoardItemLabelGroupId is not null && BoardItemLabelGroupId.Equals(BoardItemLabelGroupId) is not true) BoardItemLabelGroupId = BoardItemLabelGroupId;
        return this;
    }
}
