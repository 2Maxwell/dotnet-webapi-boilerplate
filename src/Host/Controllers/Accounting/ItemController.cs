using FSH.WebApi.Application.Accounting.ItemGroups;
using FSH.WebApi.Application.Accounting.Items;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class ItemController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search Item using available Filters.", "")]
    public Task<PaginationResponse<ItemDto>> SearchAsync(SearchItemsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get item details.", "")]
    public Task<ItemDto> GetAsync(int id)
    {
        return Mediator.Send(new GetItemRequest(id));
    }

    [HttpGet("getItemSelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search item for select.", "")]
    public Task<List<ItemSelectDto>> GetItemSelectAsync(int mandantId)
    {
        return Mediator.Send(new GetItemSelectRequest(mandantId));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new item.", "")]
    public Task<int> CreateAsync(CreateItemRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a item.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateItemRequest request, int id)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return Ok(await Mediator.Send(request));
    }
}
