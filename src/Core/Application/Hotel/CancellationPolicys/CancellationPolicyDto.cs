namespace FSH.WebApi.Application.Hotel.CancellationPolicys;

public class CancellationPolicyDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? Display { get; set; }
    public string? DisplayShort { get; set; }
    public string? DisplayHighLight { get; set; }
    public string? ConfirmationText { get; set; }
    public bool IsDefault { get; set; }
    public int FreeCancellationDays { get; set; }
    public int Priority { get; set; }
    public int NoShow { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
}
