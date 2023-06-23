using FSH.WebApi.Application.Hotel.Rooms;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class RoomsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search rooms using available filters.", "")]
    public Task<PaginationResponse<RoomDto>> SearchAsync(SearchRoomsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get room details.", "")]
    public Task<RoomDto> GetAsync(int id)
    {
        return Mediator.Send(new GetRoomRequest(id));
    }

    [HttpGet("getCountRoomsByCategoryId/{categoryId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Count rooms by categoryId.", "")]
    public Task<int> CountRoomsByCategoryIdAsync(int categoryId)
    {
        return Mediator.Send(new CountRoomsByCategoryRequest(categoryId));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new room.", "")]
    public Task<int> CreateAsync(CreateRoomRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a room.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateRoomRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("getRoomSelectDto/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search rooms for select.", "")]
    public Task<List<RoomSelectDto>> GetRoomSelectDtoAsync(int mandantId)
    {
        return Mediator.Send(new GetRoomSelectDtoRequest(mandantId));
    }

    [HttpPost("getRoomCleanReservationSelectDto")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search rooms clean/dirty with no roomReservation for select.", "")]
    public Task<List<RoomSelectDto>> GetRoomCleanReservationRoomSelectDtoAsync(GetRoomCleanReservationSelectRequest request)
    {
        return Mediator.Send(request);
    }
}
