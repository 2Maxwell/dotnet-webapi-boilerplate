using FSH.WebApi.Application.Tellus.BoardItemLabels;
using FSH.WebApi.Application.Tellus.Boards;

namespace FSH.WebApi.Application.Tellus.BoardCollections;
public class BoardCollectionDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? BoardItemLabelIds { get; set; }
    public ICollection<BoardItemLabelDto>? BoardItemLabels { get; set; }
    public string? BoardIds { get; set; }
    public ICollection<BoardDto>? Boards { get; set; }
    public bool UserOnly { get; set; }
}