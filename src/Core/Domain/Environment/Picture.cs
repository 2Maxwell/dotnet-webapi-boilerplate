using FSH.WebApi.Domain.Catalog;
using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Environment;
public class Picture : BaseEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }

    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Required]
    [StringLength(100)]
    public string? Description { get; set; }

    [Required]
    [Range(1, 500, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int OrderNumber { get; set; }

    [Required]
    [StringLength(500)]
    public string? MatchCode { get; set; }

    [Required]
    [StringLength(250)]
    public string? ImagePath { get; set; }

    public bool ShowPicture { get; set; } = true;

    public bool Publish { get; set; } = true;

    public bool DiaShow { get; set; } = true;

    public Picture(int mandantId, string? title, string? description, int orderNumber, string? matchCode, string? imagePath, bool showPicture, bool publish, bool diaShow)
    {
        MandantId = mandantId;
        Title = title;
        Description = description;
        OrderNumber = orderNumber;
        MatchCode = matchCode;
        ImagePath = imagePath;
        ShowPicture = showPicture;
        Publish = publish;
        DiaShow = diaShow;
    }

    public Picture Update(string? title, string? description, int orderNumber, string? matchCode, string? imagePath, bool showPicture, bool publish, bool diaShow)
    {
        Title = title;
        Description = description;
        OrderNumber = orderNumber;
        MatchCode = matchCode;
        if (imagePath is not null && ImagePath?.Equals(imagePath) is not true) ImagePath = imagePath;
        ShowPicture = showPicture;
        Publish = publish;
        DiaShow = diaShow;
        return this;
    }

    public Picture ClearImagePath()
    {
        ImagePath = string.Empty;
        return this;
    }


}
