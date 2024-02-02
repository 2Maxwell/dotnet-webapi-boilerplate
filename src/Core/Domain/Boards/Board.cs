using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Boards;
public class Board : AuditableEntity<int>, IAggregateRoot
{
    public Board(int mandantId, string title, string? description, string color, bool userOnly, int? boardLabelAdd, string? boardLabelRemove, bool doneBoard, int? defaultBoardItemLabelGroupId, string? source, int? sourceId)
    {
        MandantId = mandantId;
        Title = title;
        Description = description;
        Color = color;
        UserOnly = userOnly;
        BoardLabelAdd = boardLabelAdd;
        BoardLabelRemove = boardLabelRemove;
        DoneBoard = doneBoard;
        DefaultBoardItemLabelGroupId = defaultBoardItemLabelGroupId;
        Source = source;
        SourceId = sourceId;
    }

    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(25)]
    public string Title { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
    [StringLength(15)]
    public string Color { get; set; }
    public bool UserOnly { get; set; }
    public int? BoardLabelAdd { get; set; } // wird dem BoardItem hinzugefügt Dirty -> Clean
    [StringLength(60)]
    public string? BoardLabelRemove { get; set; } // LabelText mit Komma getrennt wird dem BoardItem entfernt
    public bool DoneBoard { get; set; } // wenn hier hin verschoben wird dann wird done = true gesetzt
    public int? DefaultBoardItemLabelGroupId { get; set; } // als Vorbelegung bei neuem Item
    [StringLength(15)]
    public string? Source { get; set; } // Reservation, Guest, Company, Tellus
    public int? SourceId { get; set; }

    public Board Update(string title, string? description, string color, bool userOnly, int? boardLabelAdd, string? boardLabelRemove, bool doneBoard, int? defaultBoardItemLabelGroupId)
    {
        if (title is not null && Title?.Equals(title) is not true) Title = title;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (color is not null && Color?.Equals(color) is not true) Color = color;
        UserOnly = userOnly;
        if (boardLabelAdd is not null && BoardLabelAdd?.Equals(boardLabelAdd) is not true) BoardLabelAdd = boardLabelAdd;
        if (boardLabelRemove is not null && BoardLabelRemove?.Equals(boardLabelRemove) is not true) BoardLabelRemove = boardLabelRemove;
        DoneBoard = doneBoard;
        if (defaultBoardItemLabelGroupId is not null && DefaultBoardItemLabelGroupId.Equals(defaultBoardItemLabelGroupId) is not true) DefaultBoardItemLabelGroupId = defaultBoardItemLabelGroupId;
        return this;
    }
}

public class UserExtend : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    public string? UserId { get; set; }
    public int? LastBoardCollection { get; set; }
}