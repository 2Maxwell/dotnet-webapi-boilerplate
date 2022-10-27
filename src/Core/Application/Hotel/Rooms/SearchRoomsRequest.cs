using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;

public class SearchRoomsRequest : PaginationFilter, IRequest<PaginationResponse<RoomDto>>
{
    // public int MandantId { get; set; }
    // public SearchRoomsRequest(int mandantId) => MandantId = mandantId;
}

public class RoomsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Room, RoomDto>
{
    public RoomsBySearchRequestSpec(SearchRoomsRequest request)
        : base(request) =>
        Query
        .Where(c => c.MandantId == request.MandantId)
        .Include(c => c.Category)
        .OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchRoomsRequestHandler : IRequestHandler<SearchRoomsRequest, PaginationResponse<RoomDto>>
{
    private readonly IReadRepository<Room> _repository;

    public SearchRoomsRequestHandler(IReadRepository<Room> repository) => _repository = repository;

    public async Task<PaginationResponse<RoomDto>> Handle(SearchRoomsRequest request, CancellationToken cancellationToken)
    {
        var spec = new RoomsBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<RoomDto>(list, count, request.PageNumber, request.PageSize);
    }
}