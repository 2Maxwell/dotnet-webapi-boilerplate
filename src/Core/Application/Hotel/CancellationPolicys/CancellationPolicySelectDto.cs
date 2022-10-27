namespace FSH.WebApi.Application.Hotel.CancellationPolicys;

public class CancellationPolicySelectDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
