using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;
public class SearchRoomsHousekeepingRequest : IRequest<List<RoomHousekeepingDto>>
{
    public int MandantId { get; set; }
    public bool? Clean { get; set; }
    public bool? ArrExp { get; set; }
    public bool? DepOut { get; set; }
}

public class RoomsBySearchHousekeepingRequestSpec : Specification<Room, RoomHousekeepingDto>
{
    public RoomsBySearchHousekeepingRequestSpec(SearchRoomsHousekeepingRequest request) =>
        Query
        .Where(c => c.MandantId == request.MandantId)
        .Where(c => c.Clean == request.Clean!.Value, request.Clean.HasValue)
        .Where(c => c.ArrExp == request.ArrExp!.Value, request.ArrExp.HasValue)
        .Where(c => c.DepOut == request.DepOut!.Value, request.DepOut.HasValue)
        .OrderBy(c => c.Name);
}

public class SearchRoomsHousekeepingRequestHandler : IRequestHandler<SearchRoomsHousekeepingRequest, List<RoomHousekeepingDto>>
{
    private readonly IReadRepository<Room> _repository;

    public SearchRoomsHousekeepingRequestHandler(IReadRepository<Room> repository) => _repository = repository;

    public async Task<List<RoomHousekeepingDto>> Handle(SearchRoomsHousekeepingRequest request, CancellationToken cancellationToken)
    {
        var spec = new RoomsBySearchHousekeepingRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);

        return list;
    }
}
