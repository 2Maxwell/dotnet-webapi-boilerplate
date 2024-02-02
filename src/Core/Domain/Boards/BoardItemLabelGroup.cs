    using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Boards;

public class BoardItemLabelGroup : AuditableEntity<int>, IAggregateRoot
{
    public BoardItemLabelGroup(int mandantId, string? title)
    {
        MandantId = mandantId;
        Title = title;
    }

    [Required]
    public int MandantId { get; set; }
    [StringLength(25)]
    public string? Title { get; set; }

    public BoardItemLabelGroup Update(string? title)
    {
        Title = title;
        return this;
    }
}
