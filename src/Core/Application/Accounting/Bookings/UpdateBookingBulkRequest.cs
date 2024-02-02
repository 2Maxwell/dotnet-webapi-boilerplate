using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Bookings;
public class UpdateBookingBulkRequest : IRequest<bool>
{
    public UpdateBookingBulkRequest(List<UpdateBookingRequest>? updateBookingRequestList, int mandantId)
    {
        UpdateBookingRequestList = updateBookingRequestList;
        MandantId = mandantId;
    }

    public List<UpdateBookingRequest>? UpdateBookingRequestList { get; set; }
    public int MandantId { get; set; }
}

public class UpdateBookingBulkRequestHandler : IRequestHandler<UpdateBookingBulkRequest, bool>
{
    private readonly IRepository<Booking> _repository;
    private readonly IRepository<Journal> _journalRepository;
    private readonly IRepository<MandantNumbers> _mandantNumbersRepository;
    private readonly IRepository<CashierJournal> _cashierJournalRepository;

    public UpdateBookingBulkRequestHandler(IRepository<Booking> repository, IRepository<Journal> journalRepository, IRepository<MandantNumbers> mandantNumbersRepository, IRepository<CashierJournal> cashierJournalRepository)
    {
        _repository = repository;
        _journalRepository = journalRepository;
        _mandantNumbersRepository = mandantNumbersRepository;
        _cashierJournalRepository = cashierJournalRepository;
    }

    public async Task<bool> Handle(UpdateBookingBulkRequest request, CancellationToken cancellationToken)
    {
        // GetMandantNumberRequest mNumberRequest = new(request.MandantId, "Journal");
        // GetMandantNumberRequestHandler getMandantNumberRequestHandler = new(_mandantNumbersRepository);

        foreach (var bookingRequest in request.UpdateBookingRequestList!)
        {
            var booking = await _repository.GetByIdAsync(bookingRequest.Id, cancellationToken);
            if (booking == null)
            {
                throw new NotFoundException($"UpdateBookingRequest Booking not found: {bookingRequest.Id}");
            }

            //decimal total = booking.Amount * booking.Price;
            //if ((bookingRequest.Amount * bookingRequest.Price) > total)
            //{
            //    throw new Exception($"UpdateBookingRequest Amount * Price > Total: {bookingRequest.Amount} * {bookingRequest.Price} > {total}");
            //}

            // booking.MandantId = request.MandantId;
            // booking.HotelDate = bookingRequest.HotelDate;
            booking.ReservationId = bookingRequest.ReservationId;
            booking.Name = bookingRequest.Name!;
            booking.Amount = bookingRequest.Amount;
            booking.Price = bookingRequest.Price;
            booking.Debit = bookingRequest.Debit;
            booking.ItemId = bookingRequest.ItemId;
            booking.ItemNumber = bookingRequest.ItemNumber;
            booking.Source = bookingRequest.Source!;
            booking.BookingLineNumberId = bookingRequest.BookingLineNumberId;
            booking.TaxId = bookingRequest.TaxId;
            booking.TaxRate = bookingRequest.TaxRate;
            booking.InvoicePos = bookingRequest.InvoicePos;
            booking.State = bookingRequest.State; // 1, // State
            booking.InvoiceId = bookingRequest.InvoiceId; // InvoiceId
            booking.ReferenceId = bookingRequest.ReferenceId;
            booking.KasseId = bookingRequest.KasseId;

            await _repository.UpdateAsync(booking, cancellationToken);
        }

        return true;

    }
}