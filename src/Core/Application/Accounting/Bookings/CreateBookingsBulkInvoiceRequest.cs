using FSH.WebApi.Application.Accounting.Invoices;
using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Domain.Accounting;
using Mapster;

namespace FSH.WebApi.Application.Accounting.Bookings;

// Ich glaube der RückgabeTyp muss BookingLineSummaries sein.
public class CreateBookingsBulkInvoiceRequest : IRequest<List<BookingDto>>
{
    public List<CreateBookingRequest>? CreateBookingRequestList { get; set; }
}

public class CreateBookingBulkInvoiceRequestHandler : IRequestHandler<CreateBookingsBulkInvoiceRequest, List<BookingDto>>
{
    private readonly IRepository<Booking> _repository;
    private readonly IRepository<Journal> _journalRepository;
    private readonly IRepository<MandantNumbers> _mandantNumbersRepository;

    public CreateBookingBulkInvoiceRequestHandler(IRepository<Booking> repository, IRepository<Journal> journalRepository, IRepository<MandantNumbers> mandantNumbersRepository)
    {
        _repository = repository;
        _journalRepository = journalRepository;
        _mandantNumbersRepository = mandantNumbersRepository;
    }

    public async Task<List<BookingDto>> Handle(CreateBookingsBulkInvoiceRequest request, CancellationToken cancellationToken)
    {
        int mandantId = request.CreateBookingRequestList![0].MandantId;
        GetMandantNumberRequest mNumberRequest = new(mandantId, "Journal");
        GetMandantNumberRequestHandler getMandantNumberRequestHandler = new(_mandantNumbersRepository);

        List<BookingDto> bookingsList = new();

        foreach (var bookingRequest in request.CreateBookingRequestList!)
        {
            var booking = new Booking(
                bookingRequest.MandantId,
                DateTime.Now,
                bookingRequest.HotelDate,
                bookingRequest.ReservationId,
                bookingRequest.Name!,
                bookingRequest.Amount,
                bookingRequest.Price,
                bookingRequest.Debit,
                bookingRequest.ItemId,
                bookingRequest.ItemNumber,
                bookingRequest.Source!,
                bookingRequest.BookingLineNumberId == 0 ? null : bookingRequest.BookingLineNumberId,
                bookingRequest.TaxId,
                bookingRequest.TaxRate,
                bookingRequest.InvoicePos,
                1, // State
                null, // InvoiceId
                bookingRequest.ReferenceId,
                bookingRequest.KasseId);

            var bookingSaved = await _repository.AddAsync(booking, cancellationToken);
            bookingsList.Add(bookingSaved.Adapt<BookingDto>());

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

            if (bookingRequest.ItemNumber >= 9000)
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
                    1, // State
                    journal.KasseId);

            }
        }

        return bookingsList;
    }
}
