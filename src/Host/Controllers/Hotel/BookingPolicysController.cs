using FSH.WebApi.Application.Hotel.BookingPolicys;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class BookingPolicysController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search bookingPolicys using available filters.", "")]
    public Task<PaginationResponse<BookingPolicyDto>> SearchAsync(SearchBookingPolicysRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new bookingPolicy.", "")]
    public Task<int> CreateAsync(CreateBookingPolicyRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a bookingPolicy.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBookingPolicyRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get bookingPolicy details.", "")]
    public Task<BookingPolicyDto> GetAsync(int id)
    {
        return Mediator.Send(new GetBookingPolicyRequest(id));
    }

    [HttpGet("getBookingPolicySelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search bookingPolicys for select.", "")]
    public Task<List<BookingPolicySelectDto>> GetBookingPolicySelectAsync(int mandantId)
    {
        return Mediator.Send(new GetBookingPolicySelectRequest(mandantId));
    }

    [HttpGet("getBookingPolicySelectKz/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search bookingPolicys for select Kz.", "")]
    public Task<List<BookingPolicySelectKzDto>> GetBookingPolicySelectKzAsync(int mandantId)
    {
        return Mediator.Send(new GetBookingPolicySelectKzRequest(mandantId));
    }

}
