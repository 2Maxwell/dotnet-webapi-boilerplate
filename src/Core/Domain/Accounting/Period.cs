using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;

public class Period : AuditableEntity<int>, IAggregateRoot
{
    public int MandantId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    [Required]
    [StringLength(20)]
    public string? Source { get; set; }
    [Required]
    public int SourceId { get; set; }
    public decimal Price { get; set; }
    public decimal Set { get; set; }

    public Period()
    {

    }

    public Period(int mandantId, DateTime start, DateTime end, string? source, int sourceId, decimal price, decimal set)
    {
        MandantId = mandantId;
        Start = start;
        End = end;
        Source = source;
        SourceId = sourceId;
        Price = price;
        Set = set;
    }

    public Period Update(DateTime start, DateTime end, string? source, int sourceId, decimal price, decimal set)
    {
        Start = start;
        End = end;
        Source = source;
        SourceId = sourceId;
        Price = price;
        Set = set;
        return this;
    }
}
