namespace FSH.WebApi.Application.Hotel.Periods;

public class PeriodDto : IDto
{
    public int Id { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string? Source { get; set; }
    public int SourceId { get; set; }
    public decimal Price { get; set; }
    public decimal Set { get; set; }
}
