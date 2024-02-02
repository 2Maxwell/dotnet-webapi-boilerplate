using FSH.WebApi.Application.Tellus.Boards;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search Boards using available filters.", "")]
    public Task<PaginationResponse<BoardDto>> SearchAsync(SearchBoardsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new Board.", "")]
    public Task<int> CreateAsync(CreateBoardRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get Board details.", "")]
    public Task<BoardDto> GetAsync(int id)
    {
        return Mediator.Send(new GetBoardRequest(id));
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a Board.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBoardRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getBoardSelect")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boards for select.", "")]
    public Task<List<BoardSelectDto>> GetBoardSelectDtoAsync(GetBoardSelectDtoRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("getBoardBySource")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boards by Source.", "")]
    public Task<BoardDto> GetBoardBySourceAsync(GetBoardBySourceRequest request)
    {
        return Mediator.Send(request);
    }

}
