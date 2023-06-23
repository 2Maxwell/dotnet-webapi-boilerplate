using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.RoomReservations;
public class DeleteRoomReservationRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public int ReservationId { get; set; }
}

public class DeleteRoomReservationRequestHandler : IRequestHandler<DeleteRoomReservationRequest, int>
{
    private readonly IRepository<RoomReservation> _repository;

    public DeleteRoomReservationRequestHandler(IRepository<RoomReservation> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(DeleteRoomReservationRequest request, CancellationToken cancellationToken)
    {
        var roomReservations = await _repository.ListAsync(
                       new RoomReservationByMandantIdReservationIdSpec(request.MandantId, request.ReservationId), cancellationToken);

        int counter = 0;

        foreach (var roomReservation in roomReservations)
        {
            counter++;
            await _repository.DeleteAsync(roomReservation, cancellationToken);
        }

        return counter;
    }
}
