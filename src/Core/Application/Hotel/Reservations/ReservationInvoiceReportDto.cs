namespace FSH.WebApi.Application.Hotel.Reservations;
public class ReservationInvoiceReportDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public string? RoomNumber { get; set; }
}
