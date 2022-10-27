using FSH.WebApi.Application.Accounting.ItemGroups;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class ItemGroupsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search ItemGroups using available Filters.", "")]
    public Task<PaginationResponse<ItemGroupDto>> SearchAsync(SearchItemGroupsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get itemGroup details.", "")]
    public Task<ItemGroupDto> GetAsync(int id)
    {
        return Mediator.Send(new GetItemGroupRequest(id));
    }

    [HttpGet("getItemGroupSelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search itemGroups for select.", "")]
    public Task<List<ItemGroupDto>> GetItemGroupSelectAsync(int mandantId)
    {
        return Mediator.Send(new GetItemGroupSelectRequest(mandantId));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new itemGroup.", "")]
    public Task<int> CreateAsync(CreateItemGroupRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a itemGroup.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateItemGroupRequest request, int id)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return Ok(await Mediator.Send(request));
    }
}
