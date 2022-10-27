namespace FSH.WebApi.Application.Accounting.PluGroups;

public class PluGroupDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int OrderNumber { get; set; } = 0;
}
