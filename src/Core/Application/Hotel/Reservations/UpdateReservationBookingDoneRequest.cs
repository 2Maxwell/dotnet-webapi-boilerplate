using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class UpdateReservationBookingDoneRequest : IRequest<int>
{
    public UpdateReservationBookingDoneRequest(int id, DateTime bookingDone)
    {
        Id = id;
        BookingDone = bookingDone;
    }

    public int Id { get; set; }
    public DateTime BookingDone { get; set; }
}

public class UpdateReservationBookingDoneRequestValidator : CustomValidator<UpdateReservationBookingDoneRequest>
{
    public UpdateReservationBookingDoneRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.BookingDone).NotEmpty();
    }
}

public class UpdateReservationBookingDoneRequestHandler : IRequestHandler<UpdateReservationBookingDoneRequest, int>
{
    private readonly IRepository<Reservation> _repository;

    public UpdateReservationBookingDoneRequestHandler(IRepository<Reservation> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(UpdateReservationBookingDoneRequest request, CancellationToken cancellationToken)
    {
        var res = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (res == null)
        {
            throw new NotFoundException(nameof(Reservation).ToString() + "ReservationId: " + request.Id);
        }

        var updatedResvervation = res.UpdateBookingDone(request.BookingDone);
        await _repository.UpdateAsync(res, cancellationToken);
        return res.Id;
    }
}
