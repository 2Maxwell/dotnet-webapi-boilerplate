using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Rooms;

public class GetRoomRequest : IRequest<RoomDto>
{
    public int Id { get; set; }
    public GetRoomRequest(int id) => Id = id;
}

public class GetRoomRequestHandler : IRequestHandler<GetRoomRequest, RoomDto>
{
    private readonly IRepository<Room> _repository;
    private readonly IStringLocalizer<GetRoomRequestHandler> _localizer;

    public GetRoomRequestHandler(IRepository<Room> repository, IStringLocalizer<GetRoomRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<RoomDto> Handle(GetRoomRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Room, RoomDto>)new RoomByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["room.notfound"], request.Id));
}

public class RoomByIdSpec : Specification<Room, RoomDto>, ISingleResultSpecification
{
    public RoomByIdSpec(int id) => Query.Where(x => x.Id == id);
}
