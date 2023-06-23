using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [Required]
    [StringLength(500)]
    public string? Display { get; set; }
    [StringLength(300)]
    public string? DisplayShort { get; set; }
    [StringLength(300)]
    public string? DisplayHighLight { get; set; }

    public bool CategoryIsVirtual { get; set; } = false;

    public int VirtualSourceCategoryId { get; set; } = 0;

    [StringLength(200)]
    public string? VirtualCategoryFormula { get; set; }

    public int VirtualImportCategoryId { get; set; } = 0;

    public int VirtualCategoryQuantity { get; set; } = 0;

    public int CategoryDefaultQuantity { get; set; } = 1;
    [StringLength(100)]
    public string? ChipIcon { get; set; }
    [StringLength(50)]
    public string? ChipText { get; set; }
    [StringLength(500)]
    public string? ConfirmationText { get; set; }

    public Category(int mandantId, string? kz, string? name, string? description, int orderNumber, string? properties, bool vkatRelevant, bool vkatDone, int numberOfBeds, int numberOfExtraBeds, string? display, string? displayShort, string? displayHighLight, bool categoryIsVirtual, int virtualSourceCategoryId, string? virtualCategoryFormula, int virtualImportCategoryId, int virtualCategoryQuantity, int categoryDefaultQuantity, string? chipIcon, string? chipText, string? confirmationText)
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
        Display = display;
        DisplayShort = displayShort;
        DisplayHighLight = displayHighLight;
        CategoryIsVirtual = categoryIsVirtual;
        VirtualSourceCategoryId = virtualSourceCategoryId;
        VirtualCategoryFormula = virtualCategoryFormula;
        VirtualImportCategoryId = virtualImportCategoryId;
        VirtualCategoryQuantity = virtualCategoryQuantity;
        CategoryDefaultQuantity = categoryDefaultQuantity;
        ChipIcon = chipIcon;
        ChipText = chipText;
        ConfirmationText = confirmationText;
    }

    public Category Update(string? kz, string? name, string? description, int orderNumber, string? properties, bool vkatRelevant, bool vkatDone, int numberOfBeds, int numberOfExtraBeds, string? display, string? displayShort, string? displayHighLight, bool categoryIsVirtual, int virtualSourceCategoryId, string? virtualCategoryFormula, int virtualImportCategoryId, int virtualCategoryQuantity, int categoryDefaultQuantity, string? chipIcon, string? chipText, string? confirmationText)
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
        if (display is not null && Display?.Equals(display) is not true) Display = display;
        if (displayShort is not null && DisplayShort?.Equals(displayShort) is not true) DisplayShort = displayShort;
        if (displayHighLight is not null && DisplayHighLight?.Equals(displayHighLight) is not true) DisplayHighLight = displayHighLight;
        CategoryIsVirtual = categoryIsVirtual;
        VirtualSourceCategoryId = virtualSourceCategoryId;
        VirtualCategoryFormula = virtualCategoryFormula;
        VirtualImportCategoryId = virtualImportCategoryId;
        VirtualCategoryQuantity = virtualCategoryQuantity;
        CategoryDefaultQuantity = categoryDefaultQuantity;
        if (chipIcon is not null && ChipIcon?.Equals(chipIcon) is not true) ChipIcon = chipIcon;
        if (chipText is not null && ChipText?.Equals(chipText) is not true) ChipText = chipText;
        if (confirmationText is not null && ConfirmationText?.Equals(confirmationText) is not true) ConfirmationText = confirmationText;
        return this;
    }
}
