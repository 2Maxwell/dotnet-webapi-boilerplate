using FSH.WebApi.Application.Hotel.RoomReservations;
using FSH.WebApi.Application.Hotel.Rooms;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class UpdateReservationCheckOutRequest : IRequest<bool>
{
    public int ReservationId { get; set; }
    public int MandantId { get; set; }
}

public class UpdateReservationCheckOutRequestValidator : AbstractValidator<UpdateReservationCheckOutRequest>
{
    public UpdateReservationCheckOutRequestValidator()
    {
        RuleFor(x => x.ReservationId).GreaterThan(0);
        RuleFor(x => x.MandantId).GreaterThan(0);
    }
}

public class UpdateReservationCheckOutRequestHandler : IRequestHandler<UpdateReservationCheckOutRequest, bool>
{
    private readonly IRepository<Reservation> _reservationRepository;
    private readonly IRepository<RoomReservation> _repositoryRoomReservation;
    private readonly IRepositoryWithEvents<Room> _repositoryRoom;

    public UpdateReservationCheckOutRequestHandler(IRepository<Reservation> reservationRepository, IRepository<RoomReservation> repositoryRoomReservation, IRepositoryWithEvents<Room> repositoryRoom)
    {
        _reservationRepository = reservationRepository;
        _repositoryRoomReservation = repositoryRoomReservation;
        _repositoryRoom = repositoryRoom;
    }

    public async Task<bool> Handle(UpdateReservationCheckOutRequest request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId);
        if (reservation == null)
            throw new NotFoundException("Reservation not found");

        reservation.ResKz = "O";

        reservation.DomainEvents.Add(EntityUpdatedEvent.WithEntity(reservation));
        await _reservationRepository.UpdateAsync(reservation, cancellationToken);

        SetRoomStateCheckOutRequest setRoomStateCheckOutRequest = new SetRoomStateCheckOutRequest
        {
            Id = reservation.RoomNumberId,
            MandantId = request.MandantId
        };

        SetRoomStateCheckOutRequestHandler setRoomStateCheckOutRequestHandler = new SetRoomStateCheckOutRequestHandler(_repositoryRoom);
        await setRoomStateCheckOutRequestHandler.Handle(setRoomStateCheckOutRequest, cancellationToken);

        DeleteRoomReservationRequest deleteRoomReservationRequest = new DeleteRoomReservationRequest
        {
            MandantId = request.MandantId,
            ReservationId = request.ReservationId
        };

        DeleteRoomReservationRequestHandler deleteRoomReservationRequestHandler = new DeleteRoomReservationRequestHandler(_repositoryRoomReservation);
        int counter = await deleteRoomReservationRequestHandler.Handle(deleteRoomReservationRequest, cancellationToken);

        return true;
    }
}
