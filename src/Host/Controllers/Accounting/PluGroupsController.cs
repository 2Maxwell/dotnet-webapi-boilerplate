using FSH.WebApi.Application.Accounting.PluGroups;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class PluGroupsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search PluGroups using available Filters.", "")]
    public Task<PaginationResponse<PluGroupDto>> SearchAsync(SearchPluGroupsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get pluGroup details.", "")]
    public Task<PluGroupDto> GetAsync(int id)
    {
        return Mediator.Send(new GetPluGroupRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new pluGroup.", "")]
    public Task<int> CreateAsync(CreatePluGroupRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a pluGroup.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdatePluGroupRequest request, int id)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return Ok(await Mediator.Send(request));
    }
}
