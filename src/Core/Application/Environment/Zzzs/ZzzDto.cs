namespace FSH.WebApi.Application.Environment.Zzzs;

public class ZzzDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }  
}

