using FSH.WebApi.Application.Tellus.BoardItemSubs;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardItemSubsController : VersionedApiController
{

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BoardItemSub.", "")]
    public Task<int> CreateAsync(CreateBoardItemSubRequest request)
    {
        return Mediator.Send(request);
    }

    //[HttpGet("{id:int}")]
    //[MustHavePermission(FSHAction.View, FSHResource.Brands)]
    //[OpenApiOperation("Get BoardItemSub details.", "")]
    //public Task<BoardItemSubDto> GetAsync(int id)
    //{
    //    return Mediator.Send(new GetBoardItemSubByBoardIdRequest(id));
    //}

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a BoardItemSub.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBoardItemSubRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getBoardItemSubsByBoardItemId")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boardItemSubs by boardItemId.", "")]
    public Task<List<BoardItemSubDto>> GetBoardItemSubsByBoardItemId(GetBoardItemSubByBoardIdRequest request)
    {
        return Mediator.Send(request);
    }
}
