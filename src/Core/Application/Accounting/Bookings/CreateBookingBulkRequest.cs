using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Bookings;
public class CreateBookingBulkRequest : IRequest<bool>
{
    public List<CreateBookingRequest>? CreateBookingRequestList { get; set; }
}

public class CreateBookingBulkRequestHandler : IRequestHandler<CreateBookingBulkRequest, bool>
{
    private readonly IRepository<Booking> _repository;
    public CreateBookingBulkRequestHandler(IRepository<Booking> repository) => _repository = repository;
    public async Task<bool> Handle(CreateBookingBulkRequest request, CancellationToken cancellationToken)
    {
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
                bookingRequest.BookingLineNumberId,
                bookingRequest.TaxId,
                bookingRequest.TaxRate,
                bookingRequest.InvoicePos,
                1, // State
                null, // InvoiceId
                bookingRequest.ReferenceId,
                0);

            // TODO: Add Journaleintrag mit JournalId in Booking eintragen

            await _repository.AddAsync(booking, cancellationToken);
        }

        return true;
    }
}
