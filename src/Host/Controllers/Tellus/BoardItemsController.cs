using FSH.WebApi.Application.Tellus.BoardItems;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardItemsController : VersionedApiController
{
    //[HttpPost("search")]
    //[MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    //[OpenApiOperation("Search BoardItems using available filters.", "")]
    //public Task<PaginationResponse<BoardItemDto>> SearchAsync(SearchBoardItemsRequest request)
    //{
    //    return Mediator.Send(request);
    //}

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BoardItem.", "")]
    public Task<int> CreateAsync(CreateBoardItemRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get BoardItem details.", "")]
    public Task<BoardItemDto> GetAsync(int id)
    {
        return Mediator.Send(new GetBoardItemRequest(id));
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a BoardItem.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBoardItemRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getBoardItemByBoardId")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get boardItems by BoardId.", "")]
    public Task<List<BoardItemDto>> GetBoardItemByBoardIdAsync(GetBoardItemByBoardIdRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("getBoardItemBySourceId")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get boardItems by SourceId.", "")]
    public Task<List<BoardItemDto>> GetBoardItemBySourceIdAsync(GetBoardItemBySourceIdRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("{id:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Delete a BoardItem.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteBoardItemRequest(id));
    }
}
