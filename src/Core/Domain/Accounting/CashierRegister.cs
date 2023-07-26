using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;
public class CashierRegister : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Location { get; set; }
    public decimal Stock { get; set; }
    public bool Open { get; set; }
}
