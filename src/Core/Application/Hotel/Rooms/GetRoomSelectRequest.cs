using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;
public partial class GetRoomSelectDtoRequest : IRequest<List<RoomSelectDto>>
{
    public int MandantId { get; set; }

    public GetRoomSelectDtoRequest(int mandantId)
    {
        MandantId = mandantId;
    }

    // Suchmöglichkeiten:
    // MandantId, Category, Clean, andere Reservierungen beachten
    // Blocked beachten

}

public class RoomSelectDtoSpec : Specification<Room, RoomSelectDto>, ISingleResultSpecification
{
    public RoomSelectDtoSpec(int mandantId)
    {
            Query.Where(x => x.MandantId == mandantId);
    }
}

public class GetRoomSelectDtoRequestHandler : IRequestHandler<GetRoomSelectDtoRequest, List<RoomSelectDto>>
{
    private readonly IRepository<Room> _repository;
    private readonly IStringLocalizer<GetRoomSelectDtoRequestHandler> _localizer;

    public GetRoomSelectDtoRequestHandler(IRepository<Room> repository, IStringLocalizer<GetRoomSelectDtoRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<RoomSelectDto>> Handle(GetRoomSelectDtoRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Room, RoomSelectDto>)new RoomSelectDtoSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["Room.notfound"], request.MandantId));
}