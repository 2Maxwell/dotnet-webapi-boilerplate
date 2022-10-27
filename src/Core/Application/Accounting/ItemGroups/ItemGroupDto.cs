namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class ItemGroupDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int OrderNumber { get; set; } = 0;
}
