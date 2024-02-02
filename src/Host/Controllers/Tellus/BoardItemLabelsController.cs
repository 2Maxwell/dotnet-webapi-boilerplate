using FSH.WebApi.Application.Tellus.BoardItemLabels;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardItemLabelsController : VersionedApiController
{
    //[HttpPost("search")]
    //[MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    //[OpenApiOperation("Search BoardItemLabels using available filters.", "")]
    //public Task<PaginationResponse<BoardItemLabelDto>> SearchAsync(SearchBoardItemLabelsRequest request)
    //{
    //    return Mediator.Send(request);
    //}

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BoardItemLabel.", "")]
    public Task<int> CreateAsync(CreateBoardItemLabelRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get BoardItemLabel details.", "")]
    public Task<BoardItemLabelDto> GetAsync(int id)
    {
        return Mediator.Send(new GetBoardItemLabelRequest(id));
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a BoardItemLabel.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBoardItemLabelRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getBoardItemLabelDefault")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boardItemLabels default and BoardSpecific.", "")]
    public Task<List<BoardItemLabelDto>> GetBoardItemLabelsByBoardItemId(GetBoardItemLabelDefaultRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("getBoardItemLabelByGroup")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boardItemLabels by Group.", "")]
    public Task<List<BoardItemLabelDto>> GetBoardItemLabelsByGroup(GetBoardItemLabelByGroupRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("getBoardItemLabels")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get all boardItemLabels by Mandant.", "")]
    public Task<List<BoardItemLabelDto>> GetBoardItemLabels(GetBoardItemLabelsRequest request)
    {
        return Mediator.Send(request);
    }

}
