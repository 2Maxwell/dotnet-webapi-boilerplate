using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Boards;

public class BoardCollection : AuditableEntity<int>, IAggregateRoot
{
    public BoardCollection(int mandantId, string? title, string? description, string? boardItemLabelIds, string? boardIds, bool userOnly)
    {
        MandantId = mandantId;
        Title = title;
        Description = description;
        BoardItemLabelIds = boardItemLabelIds;
        BoardIds = boardIds;
        UserOnly = userOnly;
    }

    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(25)]
    public string? Title { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
    [StringLength(400)]
    public string? BoardItemLabelIds { get; set; } // BoardItemLablesId getrennt mit Komma
    [StringLength(400)]
    public string? BoardIds { get; set; } // BoardIds getrennt mit Komma
    public bool UserOnly { get; set; }

    public BoardCollection Update(string? title, string? description, string? boardItemLabelIds, string? boardIds, bool userOnly)
    {
        if (title is not null && Title?.Equals(title) is not true) Title = title;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        BoardItemLabelIds = boardItemLabelIds;
        BoardIds = boardIds;
        UserOnly = userOnly;
        return this;
    }
}
