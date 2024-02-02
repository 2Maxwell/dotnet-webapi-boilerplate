namespace FSH.WebApi.Application.Tellus.BoardItemLabels;
public class BoardItemLabelDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Text { get; set; }
    public string? Color { get; set; }
    public bool DefaultLabel { get; set; }
    public int BoardItemLabelGroupId { get; set; }
}
