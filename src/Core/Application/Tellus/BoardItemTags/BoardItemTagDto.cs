using FSH.WebApi.Application.Tellus.BoardItemTagGroups;

namespace FSH.WebApi.Application.Tellus.BoardItemTags;
public class BoardItemTagDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Text { get; set; }
    public int BoardItemTagGroupId { get; set; }
    public string? Color { get; set; } // wird aus BoardItemTagGroup übernommen
}
