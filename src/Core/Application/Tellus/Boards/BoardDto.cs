namespace FSH.WebApi.Application.Tellus.Boards;

public class BoardDto : IDto
{
    // erstelle das BoardDto
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Color { get; set; } = null!;
    public bool UserOnly { get; set; }
    public int? BoardLabelAdd { get; set; }
    public string? BoardLabelRemove { get; set; }
    public bool DoneBoard { get; set; }
    public int DefaultBoardItemLabelGroupId { get; set; }
    public string? Source { get; set; }
    public int? SourceId { get; set; }
}
