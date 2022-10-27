using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Hotel;

public class Category : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }

    [Required]
    [StringLength(10)]
    public string? Kz { get; set; }

    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

    [Required]
    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(1, 50, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int OrderNumber { get; set; }

    [StringLength(500)]
    public string? Properties { get; set; }

    public bool VkatRelevant { get; set; } = true;

    public bool VkatDone { get; set; } = false;

    public int NumberOfBeds { get; set; } = 1;

    public int NumberOfExtraBeds { get; set; } = 0;

    [StringLength(500)]
    public string? DisplayDescriptionShort { get; set; }

    [StringLength(500)]
    public string? DisplayHightlights { get; set; }

    public bool CategoryIsVirtual { get; set; } = false;

    public int VirtualSourceCategoryId { get; set; } = 0;

    [StringLength(200)]
    public string? VirtualCategoryFormula { get; set; }

    public int VirtualImportCategoryId { get; set; } = 0;

    public int VirtualCategoryQuantity { get; set; } = 0;

    public int CategoryDefaultQuantity { get; set; } = 1;

    public Category(int mandantId,  string? kz, string? name, string? description,
        int orderNumber, string? properties, bool vkatRelevant,
        bool vkatDone, int numberOfBeds, int numberOfExtraBeds,
        string? displayDescriptionShort, string? displayHightlights,
        bool categoryIsVirtual, int virtualSourceCategoryId,
        string? virtualCategoryFormula, int virtualImportCategoryId,
        int virtualCategoryQuantity, int categoryDefaultQuantity)
    {
        MandantId = mandantId;
        Kz = kz;
        Name = name;
        Description = description;
        OrderNumber = orderNumber;
        Properties = properties;
        VkatRelevant = vkatRelevant;
        VkatDone = vkatDone;
        NumberOfBeds = numberOfBeds;
        NumberOfExtraBeds = numberOfExtraBeds;
        DisplayDescriptionShort = displayDescriptionShort;
        DisplayHightlights = displayHightlights;
        CategoryIsVirtual = categoryIsVirtual;
        VirtualSourceCategoryId = virtualSourceCategoryId;
        VirtualCategoryFormula = virtualCategoryFormula;
        VirtualImportCategoryId = virtualImportCategoryId;
        VirtualCategoryQuantity = virtualCategoryQuantity;
        CategoryDefaultQuantity = categoryDefaultQuantity;
    }

    public Category Update(string? kz, string? name, string? description,
    int orderNumber, string? properties, bool vkatRelevant,
    bool vkatDone, int numberOfBeds, int numberOfExtraBeds,
    string? displayDescriptionShort, string? displayHightlights,
    bool categoryIsVirtual, int virtualSourceCategoryId,
    string? virtualCategoryFormula, int virtualImportCategoryId,
    int virtualCategoryQuantity, int categoryDefaultQuantity)
    {
        if (kz is not null && Kz?.Equals(kz) is not true) Kz = kz;
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (orderNumber > 0 && OrderNumber != orderNumber) OrderNumber = orderNumber;
        if (properties is not null && Properties?.Equals(properties) is not true) Properties = properties;
        VkatRelevant = vkatRelevant;
        VkatDone = vkatDone;
        if (numberOfBeds > 0 && NumberOfBeds != 0) NumberOfBeds = numberOfBeds;
        NumberOfExtraBeds = numberOfExtraBeds;
        if (displayDescriptionShort is not null && DisplayDescriptionShort?.Equals(displayDescriptionShort) is not true) DisplayDescriptionShort = displayDescriptionShort;
        if (displayHightlights is not null && DisplayHightlights?.Equals(displayHightlights) is not true) DisplayHightlights = displayHightlights;
        CategoryIsVirtual = categoryIsVirtual;
        VirtualSourceCategoryId = virtualSourceCategoryId;
        VirtualCategoryFormula = virtualCategoryFormula;
        VirtualImportCategoryId = virtualImportCategoryId;
        VirtualCategoryQuantity = virtualCategoryQuantity;
        CategoryDefaultQuantity = categoryDefaultQuantity;
        return this;
    }

}
