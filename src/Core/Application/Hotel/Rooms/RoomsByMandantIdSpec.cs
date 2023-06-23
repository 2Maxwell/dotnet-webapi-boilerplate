using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;

public class RoomsByMandantIdSpec : Specification<Room>
{
    public RoomsByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId);
    }
}