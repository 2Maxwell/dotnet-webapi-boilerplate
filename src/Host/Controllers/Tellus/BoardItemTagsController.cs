using FSH.WebApi.Application.Tellus.BoardItemLabels;
using FSH.WebApi.Application.Tellus.BoardItemTags;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardItemTagsController : VersionedApiController
{
    //[HttpPost("search")]
    //[MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    //[OpenApiOperation("Search BoardItemTags using available filters.", "")]
    //public Task<PaginationResponse<BoardItemTagDto>> SearchAsync(SearchBoardItemTagsRequest request)
    //{
    //    return Mediator.Send(request);
    //}

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BoardItemTag.", "")]
    public Task<int> CreateAsync(CreateBoardItemTagRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get BoardItemTag details.", "")]
    public Task<BoardItemTagDto> GetAsync(int id)
    {
        return Mediator.Send(new GetBoardItemTagRequest(id));
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a BoardItemTag.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBoardItemTagRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getBoardItemTagsByGroup")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boardItemTags for select.", "")]
    public Task<List<BoardItemTagDto>> GetBoardItemTagsByGroupAsync(GetBoardItemTagsByGroupRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("getBoardItemTags")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get all boardItemTags by Mandant.", "")]
    public Task<List<BoardItemTagDto>> GetBoardItemTags(GetBoardItemTagsRequest request)
    {
        return Mediator.Send(request);
    }
}
