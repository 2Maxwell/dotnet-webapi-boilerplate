using FSH.WebApi.Application.Hotel.Reservations;
using FSH.WebApi.Domain.Helper;

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

    [HttpPost("getReservationsByGroupMasterId")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Get a List of reservations by GroupMasterId.", "")]
    public Task<List<ReservationListDto>> GetReservationsByGroupIdAsync(GetReservationsByGroupIdRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("getReservationsByResKz")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Get a List of reservations by ResKz.", "")]
    public Task<List<ReservationDto>> GetReservationsByResKzAsync(GetReservationsByResKzRequest request)
    {
        return Mediator.Send(request);
    }


    [HttpPost("getReservationsByMatchCode")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Get a List of reservations by MatchCode.", "")]
    public Task<List<ReservationListDto>> GetReservationsByMatchCodeAsync(GetReservationsByMatchCodeRequest request)
    {
        return Mediator.Send(request);
    }


    [HttpPost("changeTransferForGroupmembersByGroupMasterId")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Change Transfer for Groupmembers by GroupMasterId.", "")]
    public Task<int> ChangeTransferByGroupIdAsync(UpdateReservationsTransferRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("getReservationPackageCalculation")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Gets List of BookingLineSummaries for Reservation Packages.", "")]
    public Task<List<BookingLineSummary>> GetReservationPackageCalulationAsync(GetReservationPackageCalculationRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{mandantId:int}&{date:dateTime}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get reservationStatArrival.", "")]
    public Task<ReservationStatArrivalDto> GetReservationStatArrivalAsync(int mandantId, DateTime date)
    {
        return Mediator.Send(new GetReservationStatArrival(mandantId, date));
    }

    [HttpPost("updateReservationCheckOut")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("CheckOut Reservation.", "")]
    public Task<bool> ReservationCheckOutRequestAsync(UpdateReservationCheckOutRequest request)
    {
        return Mediator.Send(request);
    }

}
