namespace FSH.WebApi.Application.Tellus.BoardItemAttachments;
public class BoardItemAttachmentDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int BoardItemId { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string? Path { get; set; }
}
