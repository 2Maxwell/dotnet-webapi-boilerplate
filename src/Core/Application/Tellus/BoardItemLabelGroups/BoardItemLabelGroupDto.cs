using FSH.WebApi.Application.Tellus.BoardItemLabels;

namespace FSH.WebApi.Application.Tellus.BoardItemLabelGroups;
public class BoardItemLabelGroupDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Title { get; set; }
    public List<BoardItemLabelDto>? BoardItemLabels { get; set; }
}
