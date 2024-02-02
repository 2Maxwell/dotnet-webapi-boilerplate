using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Bookings;
public class CreateBookingBulkRequest : IRequest<bool>
{
    public CreateBookingBulkRequest(List<CreateBookingRequest>? createBookingRequestList, int mandantId)
    {
        CreateBookingRequestList = createBookingRequestList;
        MandantId = mandantId;
    }

    // Diese Request kann nur für Buchung auf Konten Gast im Haus genutzt werden.
    // Bei Buchungen die eine Rechnung zur Folge haben muss der RückgabeTyp
    // List<Booking> sein. Dann kann die Rechnung erstellt werden und die Buchungen
    // sind mit der Id gekennzeichnet.

    public List<CreateBookingRequest>? CreateBookingRequestList { get; set; }
    public int MandantId { get; set; }
}

public class CreateBookingBulkRequestHandler : IRequestHandler<CreateBookingBulkRequest, bool>
{
    private readonly IRepository<Booking> _repository;
    private readonly IRepository<Journal> _journalRepository;
    private readonly IRepository<MandantNumbers> _mandantNumbersRepository;
    private readonly IRepository<CashierJournal> _cashierJournalRepository;

    public CreateBookingBulkRequestHandler(IRepository<Booking> repository, IRepository<Journal> journalRepository, IRepository<MandantNumbers> mandantNumbersRepository, IRepository<CashierJournal> cashierJournalRepository)
    {
        _repository = repository;
        _journalRepository = journalRepository;
        _mandantNumbersRepository = mandantNumbersRepository;
        _cashierJournalRepository = cashierJournalRepository;
    }

    public async Task<bool> Handle(CreateBookingBulkRequest request, CancellationToken cancellationToken)
    {
        GetMandantNumberRequest mNumberRequest = new(request.MandantId, "Journal");
        GetMandantNumberRequestHandler getMandantNumberRequestHandler = new(_mandantNumbersRepository);

        foreach (var bookingRequest in request.CreateBookingRequestList!)
        {
            var booking = new Booking(
                request.MandantId,
                DateTime.Now,
                bookingRequest.HotelDate,
                bookingRequest.ReservationId,
                bookingRequest.Name!,
                bookingRequest.Amount,
                Math.Round(bookingRequest.Price, 2), // Price,
                bookingRequest.Debit,
                bookingRequest.ItemId,
                bookingRequest.ItemNumber,
                bookingRequest.Source!,
                bookingRequest.BookingLineNumberId,
                bookingRequest.TaxId,
                bookingRequest.TaxRate,
                bookingRequest.InvoicePos,
                bookingRequest.State, // 1, // State
                null, // InvoiceId
                bookingRequest.ReferenceId,
                bookingRequest.KasseId);

            await _repository.AddAsync(booking, cancellationToken);

            var journal = new Journal(
                    booking.MandantId,
                    await getMandantNumberRequestHandler.Handle(mNumberRequest, cancellationToken),
                    booking.Id,
                    DateTime.Now,
                    booking.HotelDate,
                    booking.ReservationId,
                    booking.Name,
                    booking.Amount,
                    booking.Price,
                    booking.Debit,
                    booking.ItemId,
                    booking.ItemNumber,
                    booking.Source,
                    booking.BookingLineNumberId,
                    booking.TaxId,
                    booking.TaxRate,
                    booking.State,
                    booking.ReferenceId,
                    booking.KasseId);

            await _journalRepository.AddAsync(journal, cancellationToken);

            if (bookingRequest.ItemNumber >= 8999)
            {
                var cashierJournal = new CashierJournal(
                    booking.MandantId,
                    journal.Id,
                    journal.JournalIdMandant,
                    journal.BookingId,
                    null,
                    null,
                    journal.JournalDate,
                    journal.HotelDate,
                    journal.Name,
                    journal.Amount,
                    journal.Price,
                    journal.Debit,
                    journal.ItemId,
                    journal.ItemNumber,
                    journal.Source,
                    journal.State, // 1, // State 1 = offen, wird bei Create eingetragen
                    DateTime.Now,
                    journal.KasseId);

                await _cashierJournalRepository.AddAsync(cashierJournal, cancellationToken);
            }

            // TODO: Wenn ItemNumber >= 9000 dann Buchung in CashierJournal eintragen
            // normalerweise sollte bei einer Zahlung für eine Rechnung die Rechnungs-
            // Nummer mit eingetragen werden. Keine Ahnung wie das gehen soll.

            // NOTE nach dem RechnungsVorgang wenn RG OK ist dann werden die Buchungen
            // mit ItemNumber >= 9000 in das CashierJournal eingetragen. Dann steht auch
            // die Rechnungsnummer fest.

            // NOTE Für CashierJournal ist zwingend die KassenId notwendig also wird auch in Bookings
            // die KassenId eingetragen.
        }

        return true;
    }
}
