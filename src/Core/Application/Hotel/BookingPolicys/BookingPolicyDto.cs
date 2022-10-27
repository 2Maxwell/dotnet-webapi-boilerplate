namespace FSH.WebApi.Application.Hotel.BookingPolicys;

public class BookingPolicyDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? DisplayShort { get; set; }
    public string? Display { get; set; }
    public string? ConfirmationText { get; set; }
    public bool IsDefault { get; set; }
    public bool CreditCard { get; set; }
    public bool Deposit { get; set; }
    public int Priority { get; set; }
}
