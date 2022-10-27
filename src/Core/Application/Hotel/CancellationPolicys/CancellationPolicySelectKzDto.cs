namespace FSH.WebApi.Application.Hotel.CancellationPolicys;

public class CancellationPolicySelectKzDto : IDto
{
    public int Id { get; set; }
    public string? Kz { get; set; }
}
