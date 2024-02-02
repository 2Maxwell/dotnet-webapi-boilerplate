using FSH.WebApi.Application.Tellus.BoardItemLabelGroups;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardItemLabelGroupsController : VersionedApiController
{
    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BoardItemLabelGroup.", "")]
    public Task<int> CreateAsync(CreateBoardItemLabelGroupRequest request)
    {
        return Mediator.Send(request);
    }

    //[HttpGet("{id:int}")]
    //[MustHavePermission(FSHAction.View, FSHResource.Brands)]
    //[OpenApiOperation("Get BoardItemLabelGroup details.", "")]
    //public Task<BoardItemLabelGroupDto> GetAsync(int id)
    //{
    //    return Mediator.Send(new GetBoardItemLabelGroupRequest(id));
    //}

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a BoardItemLabelGroup.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBoardItemLabelGroupRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getBoardItemLabelGroupSelect")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boardItemLabelGroups for select.", "")]
    public Task<List<BoardItemLabelGroupSelectDto>> GetBoardItemLabelGroupSelectAsync(GetBoardItemLabelGroupSelectRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("getBoardItemLabelGroups")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boardItemLabelGroups.", "")]
    public Task<List<BoardItemLabelGroupDto>> GetBoardItemLabelGroupsAsync(GetBoardItemLabelGroupsRequest request)
    {
        return Mediator.Send(request);
    }
}
