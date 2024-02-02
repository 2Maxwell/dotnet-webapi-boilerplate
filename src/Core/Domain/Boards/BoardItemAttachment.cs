using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Boards;

public class BoardItemAttachment : AuditableEntity<int>, IAggregateRoot
{
    public BoardItemAttachment(int mandantId, int boardItemId, string? fileName, string? contentType, string? path)
    {
        MandantId = mandantId;
        BoardItemId = boardItemId;
        FileName = fileName;
        ContentType = contentType; // jpeg, doc, docx, xls, xlsx, bmp, tif, png 
        Path = path;
    }

    [Required]
    public int MandantId { get; set; }
    [Required]
    public int BoardItemId { get; set; }
    [Required]
    [StringLength(100)]
    public string? FileName { get; set; }
    [Required]
    [StringLength(100)]
    public string? ContentType { get; set; }
    [Required]
    [StringLength(150)]
    public string? Path { get; set; }

    public BoardItemAttachment Update(string? contentType)
    {
        ContentType = contentType;
        return this;
    }
}
