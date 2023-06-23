using FSH.WebApi.Application.Hotel.Reservations;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class ReservationsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search reservations using available filters.", "")]
    public Task<PaginationResponse<ReservationListDto>> SearchAsync(SearchReservationsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}&{mandantId:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get reservation details.", "")]
    public Task<ReservationDto> GetAsync(int id, int mandantId)
    {
        return Mediator.Send(new GetReservationRequest(id, mandantId));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new reservation.", "")]
    public Task<int> CreateAsync(CreateReservationRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("createReservationByCartMandantCartItem")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new reservation by CartMandant CartItem.", "")]
    public Task<int> CreateReservationByCartMandantCartItemAsync(CreateReservationByCartMandantRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a reservation.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateReservationRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }
}
