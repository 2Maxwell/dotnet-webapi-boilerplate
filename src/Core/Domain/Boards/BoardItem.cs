using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Boards;

public class BoardItem : AuditableEntity<int>, IAggregateRoot
{
    public BoardItem(int mandantId, string? boardItemLabelIds, string? boardItemTagIds, string? title, string? description, int? boardId, DateTime? start, DateTime? end, int? boardItemTypeEnumId, string? boardSourceIdJson, bool isTemplate, bool? fixedBoard, string? imageName, string? imagePath, string? repeatMatchCode, string? boardItemRepeaterJson, string? sourceLink, int? defaultBoardItemLabelGroupId)
    {
        MandantId = mandantId;
        BoardItemLabelIds = boardItemLabelIds;
        BoardItemTagIds = boardItemTagIds;
        Title = title;
        Description = description;
        BoardId = boardId;
        Start = start;
        End = end;
        BoardItemTypeEnumId = boardItemTypeEnumId;
        BoardSourceIdJson = boardSourceIdJson;
        IsTemplate = isTemplate;
        FixedBoard = fixedBoard;
        ImageName = imageName;
        ImagePath = imagePath;
        RepeatMatchCode = repeatMatchCode;
        BoardItemRepeaterJson = boardItemRepeaterJson;
        SourceLink = sourceLink;
        DefaultBoardItemLabelGroupId = defaultBoardItemLabelGroupId;
    }

    [Required]
    public int MandantId { get; set; }
    [StringLength(400)]
    public string? BoardItemLabelIds { get; set; }
    [StringLength(400)]
    public string? BoardItemTagIds { get; set; }
    [Required]
    [StringLength(40)]
    public string? Title { get; set; }
    [StringLength(200)]
    public string? Description { get; set; }
    public int? BoardId { get; set; } // BoardItem soll auch in anderen Teilen des Programm angezeigt werden. z.B. OptionFollowUp, allg. für Termine, In der VKAT für MesseInfo, ... Daher nicht mit Required markiert.
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public int? BoardItemTypeEnumId { get; set; }
    [StringLength(300)]
    public string? BoardSourceIdJson { get; set; } // ResId, GuestId, BookerId, CompanyId, ContactId, ...

    //// [NotMapped]
    //public virtual BoardSourceId? BoardSourceId { get; set; }
    public bool IsTemplate { get; set; } // dafür muss eine Funktion "SAVE AS TEMPlATE" vorhanden sein
    //[StringLength(400)]
    //public string? BoardItemAttachmentIds { get; set; }
    public bool? FixedBoard { get; set; } // kann nicht auf ein anderes Board verschoben werden
    [StringLength(40)]
    public string? ImageName { get; set; }
    [StringLength(200)]
    public string? ImagePath { get; set; }

    // MatchCode für die Wiederholung: Irgendwie muss man erkennen das der Repeat schon angelegt ist.
    // Der RepeatMatchCode wird bei der Erstellung des Repeats erzeugt und bei jedem Repeat wieder verwendet.
    // Es wird immer vom letzten erstellten Repeat ausgegangen. Und von diesem aus wird dann das nächste Repeat erzeugt.
    [StringLength(40)]
    public string? RepeatMatchCode { get; set; }

    [StringLength(400)]
    public string? BoardItemRepeaterJson { get; set; }

    // [NotMapped]
    // public virtual BoardItemRepeater? BoardItemRepeater { get; set; }
    [StringLength(150)]
    public string? SourceLink { get; set; } // Link zu einer Reservierung, Guest, Booker, Company, Contact, ...
    [Required]
    public int? DefaultBoardItemLabelGroupId { get; set; }


    public BoardItem Update(string? boardItemLabelIds, string? boardItemTagIds, string? title, string? description, int? boardId, DateTime? start, DateTime? end, int? boardItemTypeEnumId, string? boardSourceIdJson, bool isTemplate, bool? fixedBoard, string? imageName, string? imagePath, string? repeatMatchCode, string? boardItemRepeaterJson, string? sourceLink, int? defaultBoardItemLabelGroupId)
    {
        BoardItemLabelIds = boardItemLabelIds;
        BoardItemTagIds = boardItemTagIds;
        if (title is not null && Title?.Equals(title) is not true) Title = title;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        BoardId = boardId;
        if (start is not null && Start?.Equals(start) is not true) Start = start;
        if (end is not null && End?.Equals(end) is not true) End = end;
        BoardItemTypeEnumId = boardItemTypeEnumId;
        BoardSourceIdJson = boardSourceIdJson;
        IsTemplate = isTemplate;
        FixedBoard = fixedBoard;
        if (imageName is not null && ImageName?.Equals(imageName) is not true) ImageName = imageName;
        if (imagePath is not null && ImagePath?.Equals(imagePath) is not true) ImagePath = imagePath;
        if (repeatMatchCode is not null && RepeatMatchCode?.Equals(repeatMatchCode) is not true) RepeatMatchCode = repeatMatchCode;
        BoardItemRepeaterJson = boardItemRepeaterJson;
        if (sourceLink is not null && SourceLink?.Equals(sourceLink) is not true) SourceLink = sourceLink;
        if (defaultBoardItemLabelGroupId is not null && DefaultBoardItemLabelGroupId?.Equals(defaultBoardItemLabelGroupId) is not true) DefaultBoardItemLabelGroupId = defaultBoardItemLabelGroupId;
        return this;
    }
}
