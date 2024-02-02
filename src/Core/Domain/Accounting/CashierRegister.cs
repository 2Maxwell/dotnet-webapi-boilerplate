using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;
public class CashierRegister : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(50)]
    public string? Name { get; set; }
    [StringLength(50)]
    public string? Location { get; set; }
    public decimal Stock { get; set; }
    public bool Open { get; set; }

    public CashierRegister(int mandantId, string? name, string? location, decimal stock, bool open)
    {
        MandantId = mandantId;
        Name = name;
        Location = location;
        Stock = stock;
        Open = open;
    }

    public CashierRegister Update(int mandantId, string? name, string? location, decimal stock, bool open)
    {
        MandantId = mandantId;
        Name = name;
        Location = location;
        Stock = stock;
        Open = open;
        return this;
    }
}
