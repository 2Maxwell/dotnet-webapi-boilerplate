using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.RoomReservations;

public class RoomReservationByMandantIdReservationIdSpec : Specification<RoomReservation>
{
    public RoomReservationByMandantIdReservationIdSpec(int mandantId, int reservationId) =>
        Query
        .Where(x => x.MandantId == mandantId && x.ReservationId == reservationId);
}