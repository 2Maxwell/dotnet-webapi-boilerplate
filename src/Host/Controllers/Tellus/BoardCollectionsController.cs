using FSH.WebApi.Application.Tellus.BoardCollections;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardCollectionsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search BoardCollections using available filters.", "")]
    public Task<PaginationResponse<BoardCollectionDto>> SearchAsync(SearchBoardCollectionsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BoardCollection.", "")]
    public Task<int> CreateAsync(CreateBoardCollectionRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get BoardCollection details.", "")]
    public Task<BoardCollectionDto> GetAsync(int id)
    {
        return Mediator.Send(new GetBoardCollectionRequest(id));
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a BoardCollection.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBoardCollectionRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getBoardCollectionSelectDto")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boardCollections for select.", "")]
    public Task<List<BoardCollectionSelectDto>> GetBoardCollectionSelectDtoAsync(GetBoardCollectionSelectDtoRequest request)
    {
        return Mediator.Send(request);
    }

}
