using FSH.WebApi.Application.Hotel.CancellationPolicys;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class CancellationPolicysController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search CancellationPolicys using available filters.", "")]
    public Task<PaginationResponse<CancellationPolicyDto>> SearchAsync(SearchCancellationPolicysRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new CancellationPolicy.", "")]
    public Task<int> CreateAsync(CreateCancellationPolicyRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a CancellationPolicy.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateCancellationPolicyRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get CancellationPolicy details.", "")]
    public Task<CancellationPolicyDto> GetAsync(int id)
    {
        return Mediator.Send(new GetCancellationPolicyRequest(id));
    }

    [HttpGet("getCancellationPolicySelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search bookingPolicys for select.", "")]
    public Task<List<CancellationPolicySelectDto>> GetCancellationPolicySelectAsync(int mandantId)
    {
        return Mediator.Send(new GetCancellationPolicySelectRequest(mandantId));
    }

    [HttpGet("getCancellationPolicySelectKz/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search bookingPolicys for select Kz.", "")]
    public Task<List<CancellationPolicySelectKzDto>> GetCancellationPolicySelectKzAsync(int mandantId)
    {
        return Mediator.Send(new GetCancellationPolicySelectKzRequest(mandantId));
    }

}
