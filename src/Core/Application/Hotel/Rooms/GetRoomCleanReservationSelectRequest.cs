using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;
public class GetRoomCleanReservationSelectRequest : IRequest<List<RoomSelectDto>>
{
    public GetRoomCleanReservationSelectRequest(int mandantId, int categoryId, DateTime arrival, DateTime departure, bool clean)
    {
        MandantId = mandantId;
        CategoryId = categoryId;
        Arrival = arrival;
        Departure = departure;
        Clean = clean;
    }

    public int MandantId { get; set; }
    public int CategoryId { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public bool Clean { get; set; }
}

public class GetRoomCleanReservationSelectRequestHandler : IRequestHandler<GetRoomCleanReservationSelectRequest, List<RoomSelectDto>>
{
    private readonly IDapperRepository _repository;
    private readonly IStringLocalizer<GetRoomCleanReservationSelectRequestHandler> _localizer;

    public GetRoomCleanReservationSelectRequestHandler(IDapperRepository repository, IStringLocalizer<GetRoomCleanReservationSelectRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<RoomSelectDto>> Handle(GetRoomCleanReservationSelectRequest request, CancellationToken cancellationToken)
    {
        DateTime departure_1 = request.Departure.AddDays(-1);

        string sqlClean = $"SELECT * FROM lnx.Room WHERE " +
            $"NOT EXISTS (SELECT RoomId FROM lnx.RoomReservation WHERE Id = RoomId " +
            $"AND Occupied BETWEEN '{request.Arrival}' AND '{departure_1.Date}') AND (CategoryId = {request.CategoryId}) " +
            $"AND (Clean = 1 AND " +
            $"((Blocked = 0) OR " +
            $"(Blocked = 1 AND BlockedEnd <= '{request.Arrival}') OR " +
            $"(Blocked = 1 AND BlockedStart >= '{request.Departure}')))";
        // sqlClean string in Linq
        // var rooms = await _repository.QueryAsync<Room>(r => !r.RoomReservations.Any(rr => rr.Occupied >= request.Arrival && rr.Occupied <= departure_1.Date) && r.CategoryId == request.CategoryId && r.Clean && (!r.Blocked || r.BlockedEnd <= request.Arrival || r.BlockedStart >= request.Departure), cancellationToken: cancellationToken);

        string sqlCleanDirty = $"SELECT * FROM lnx.Room WHERE " +
            $"NOT EXISTS (SELECT RoomId FROM lnx.RoomReservation WHERE Id = RoomId " +
            $"AND Occupied BETWEEN '{request.Arrival}' AND '{departure_1.Date}') AND (CategoryId = {request.CategoryId}) AND " +
            $"((Blocked = 0) OR " +
            $"(Blocked = 1 AND BlockedEnd <= '{request.Arrival}') OR " +
            $"(Blocked = 1 AND BlockedStart >= '{request.Departure}'))";

        string sql = request.Clean ? sqlClean : sqlCleanDirty;

        var rooms = await _repository.QueryAsync<Room>(sql, cancellationToken: cancellationToken);
        _ = rooms ?? throw new NotFoundException(_localizer["Rooms in CategoryId ({0}).notfound.", request.CategoryId]);

        List<RoomSelectDto> list = new List<RoomSelectDto>();

        foreach (var room in rooms)
        {
            RoomSelectDto rs = new RoomSelectDto
            {
                Id = room.Id,
                CategoryId = room.CategoryId,
                Name = room.Name,
                Beds = room.Beds,
                BedsExtra = room.BedsExtra,
                Clean = room.Clean,
            };
            list.Add(rs);
        }

        return list;
    }
}
