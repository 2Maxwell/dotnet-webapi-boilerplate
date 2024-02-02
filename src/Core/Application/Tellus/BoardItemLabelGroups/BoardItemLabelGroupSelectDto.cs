namespace FSH.WebApi.Application.Tellus.BoardItemLabelGroups;
public class BoardItemLabelGroupSelectDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Title { get; set; }
}
