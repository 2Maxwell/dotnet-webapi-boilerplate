using FSH.WebApi.Application.Tellus.BoardItemTagGroups;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardItemTagGroupsController : VersionedApiController
{
    //[HttpPost("search")]
    //[MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    //[OpenApiOperation("Search BoardItemTagGroups using available filters.", "")]
    //public Task<PaginationResponse<BoardItemTagGroupDto>> SearchAsync(SearchBoardItemTagGroupsRequest request)
    //{
    //    return Mediator.Send(request);
    //}

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BoardItemTagGroup.", "")]
    public Task<int> CreateAsync(CreateBoardItemTagGroupRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get BoardItemTagGroup details.", "")]
    public Task<BoardItemTagGroupDto> GetAsync(int id)
    {
        return Mediator.Send(new GetBoardItemTagGroupRequest(id));
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a BoardItemTagGroup.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBoardItemTagGroupRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getBoardItemTagGroupSelect")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boardItemTagGroups for select.", "")]
    public Task<List<BoardItemTagGroupDto>> GetBoardItemTagGroupSelectAsync(GetBoardItemTagGroupSelectRequest request)
    {
        return Mediator.Send(request);
    }
}
