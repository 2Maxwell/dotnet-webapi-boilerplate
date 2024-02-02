using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Boards;
public class BoardItemTag : AuditableEntity<int>, IAggregateRoot
{
    public BoardItemTag(int mandantId, string? text, int boardItemTagGroupId)
    {
        MandantId = mandantId;
        Text = text;
        BoardItemTagGroupId = boardItemTagGroupId;
    }

    // BoardItemTag sind auf den Mandanten bezogene Tags, die in festen Gruppen geordnet sind:
    // Zimmer, Gastwünsche, etc.
    [Required]
    public int MandantId { get; set; }
    [StringLength(20)]
    public string? Text { get; set; }
    [Required]
    public int BoardItemTagGroupId { get; set; }
    //[ForeignKey("BoardItemTagGroupId")]
    //public BoardItemTagGroup? BoardItemTagGroup { get; set; }

    public BoardItemTag Update(string? text, int boardItemTagGroupId)
    {
        Text = text;
        BoardItemTagGroupId = boardItemTagGroupId;
        return this;
    }
}
