using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Boards;

public class BoardItemTagGroup : AuditableEntity<int>, IAggregateRoot
{
    public BoardItemTagGroup(int mandantId, string? title, string? color)
    {
        MandantId = mandantId;
        Title = title;
        Color = color;
    }

    [Required]
    public int MandantId { get; set; }
    [StringLength(25)]
    public string? Title { get; set; }
    [Required]
    [StringLength(25)]
    public string? Color { get; set; }

    public BoardItemTagGroup Update(string? title, string? color)
    {
        Title = title;
        Color = color;
        return this;
    }
}