namespace FSH.WebApi.Application.Hotel.Categorys;

public class CategoryDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Kz { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int OrderNumber { get; set; }
    public string? Properties { get; set; }
    public bool VkatRelevant { get; set; }
    public bool VkatDone { get; set; }
    public int NumberOfBeds { get; set; }
    public int NumberOfExtraBeds { get; set; }
    public string? DisplayDescriptionShort { get; set; }
    public string? DisplayHightlights { get; set; }
    public bool CategoryIsVirtual { get; set; }
    public int VirtualSourceCategoryId { get; set; }
    public string? VirtualCategoryFormula { get; set; }
    public int VirtualImportCategoryId { get; set; }
    public int VirtualCategoryQuantity { get; set; }
    public int CategoryDefaultQuantity { get; set; }
}
