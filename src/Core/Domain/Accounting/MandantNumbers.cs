namespace FSH.WebApi.Domain.Accounting;
public class MandantNumbers : AuditableEntity<int>, IAggregateRoot
{
    public int MandantId { get; set; }
    public int GroupMasterNumber { get; set; }
    public int PhantomNumber { get; set; }
    public int InvoiceNumber { get; set; }
    public int JournalNumber { get; set; }
    public int ReservationNumber { get; set; }

    public MandantNumbers(int mandantId, int groupMasterNumber, int phantomNumber, int invoiceNumber, int journalNumber, int reservationNumber)
    {
        MandantId = mandantId;
        GroupMasterNumber = groupMasterNumber;
        PhantomNumber = phantomNumber;
        InvoiceNumber = invoiceNumber;
        JournalNumber = journalNumber;
        ReservationNumber = reservationNumber;
    }

    public MandantNumbers UpdateGroupMasterNumber()
    {
        GroupMasterNumber++;
        return this;
    }

    public MandantNumbers UpdatePhantomNumber()
    {
        PhantomNumber++;
        return this;
    }

    public MandantNumbers UpdateInvoiceNumber()
    {
        InvoiceNumber++;
        return this;
    }

    public MandantNumbers UpdateJournalNumber()
    {
        JournalNumber++;
        return this;
    }

    public MandantNumbers UpdateReservationNumber()
    {
        ReservationNumber++;
        return this;
    }

}
