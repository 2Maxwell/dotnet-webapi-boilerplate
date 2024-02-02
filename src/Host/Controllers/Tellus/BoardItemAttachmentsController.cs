using FSH.WebApi.Application.Tellus.BoardItemAttachments;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardItemAttachmentsController : VersionedApiController
{
    //[HttpPost("search")]
    //[MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    //[OpenApiOperation("Search BoardItemAttachments using available filters.", "")]
    //public Task<PaginationResponse<BoardItemAttachmentDto>> SearchAsync(SearchBoardItemAttachmentsRequest request)
    //{
    //    return Mediator.Send(request);
    //}

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BoardItemAttachment.", "")]
    public Task<int> CreateAsync(CreateBoardItemAttachmentRequest request)
    {
        return Mediator.Send(request);
    }

    //[HttpGet("{id:int}")]
    //[MustHavePermission(FSHAction.View, FSHResource.Brands)]
    //[OpenApiOperation("Get BoardItemAttachment details.", "")]
    //public Task<BoardItemAttachmentDto> GetAsync(int id)
    //{
    //    return Mediator.Send(new GetBoardItemAttachmentRequest(id));
    //}

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a BoardItemAttachment.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBoardItemAttachmentRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getBoardItemAttachmentsByBoardItemId")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search boardItemAttachments by boardItemId.", "")]
    public Task<List<BoardItemAttachmentDto>> GetBoardItemAttachmentByBoardItemId(GetBoardItemAttachmentByBoardItemIdRequest request)
    {
        return Mediator.Send(request);
    }
}
