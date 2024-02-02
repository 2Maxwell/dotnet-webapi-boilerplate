namespace FSH.WebApi.Application.Accounting.CashierRegisters;
public class CashierRegisterDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public decimal Stock { get; set; }
    public bool Open { get; set; }
}
