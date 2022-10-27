using FSH.WebApi.Application.Accounting.Merchandises;
using FSH.WebApi.Application.Accounting.PluGroups;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class MerchandisesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search Merchandises using available Filters.", "")]
    public Task<PaginationResponse<MerchandiseDto>> SearchAsync(SearchMerchandisesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get merchandise details.", "")]
    public Task<MerchandiseDto> GetAsync(int id)
    {
        return Mediator.Send(new GetMerchandiseRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new merchandise.", "")]
    public Task<int> CreateAsync(CreateMerchandiseRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a merchandise.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateMerchandiseRequest request, int id)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return Ok(await Mediator.Send(request));
    }
}
