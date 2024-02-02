using FSH.WebApi.Application.Tellus.BoardItemLabels;
using FSH.WebApi.Application.Tellus.BoardItemSubs;
using FSH.WebApi.Application.Tellus.BoardItemTags;
using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.BoardItems;
public class BoardItemDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? BoardItemLabelIds { get; set; }
    public List<BoardItemLabelDto> ItemLabels { get; set; } = new List<BoardItemLabelDto>();
    public string? BoardItemTagIds { get; set; }
    public List<BoardItemTagDto> ItemTags { get; set; } = new List<BoardItemTagDto>();
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? BoardId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public int? BoardItemTypeEnumId { get; set; }
    public string? BoardSourceIdJson { get; set; }
    public List<BoardSourceId>? BoardSourceIds { get; set; }
    public bool IsTemplate { get; set; }
    public bool? FixedBoard { get; set; }
    public string? ImageName { get; set; }
    public string? ImagePath { get; set; }
    public string? RepeatMatchCode { get; set; }
    public string? BoardItemRepeaterJson { get; set; }
    public BoardItemRepeater? BoardItemRepeater { get; set; }
    public string? SourceLink { get; set; }
    public int? DefaultBoardItemLabelGroupId { get; set; }
    public List<BoardItemSubDto> ItemSubs { get; set; } = new List<BoardItemSubDto>();
}
