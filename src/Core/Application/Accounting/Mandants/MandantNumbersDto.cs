namespace FSH.WebApi.Application.Accounting.Mandants;
public class MandantNumbersDto : IDto
{
    public int GroupMasterNumber { get; set; }
    public int PhantomNumber { get; set; }
    public int InvoiceNumber { get; set; }
    public int JournalNumber { get; set; }
    public int ReservationNumber { get; set; }
}
