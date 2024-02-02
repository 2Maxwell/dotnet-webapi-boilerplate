using FSH.WebApi.Domain.Boards;
using Mapster;

namespace FSH.WebApi.Application.Tellus.BoardItemAttachments;
public class GetBoardItemAttachmentByBoardItemIdRequest : IRequest<List<BoardItemAttachmentDto>>
{
    public int MandantId { get; set; }
    public int BoardItemId { get; set; }
    public GetBoardItemAttachmentByBoardItemIdRequest(int mandantId, int boardItemId)
    {
        MandantId = mandantId;
        BoardItemId = boardItemId;
    }
}

public class GetBoardItemAttachmentByBoardIdRequestHandler : IRequestHandler<GetBoardItemAttachmentByBoardItemIdRequest, List<BoardItemAttachmentDto>>
{
    private readonly IRepository<BoardItemAttachment> _repository;
    private readonly IStringLocalizer<GetBoardItemAttachmentByBoardIdRequestHandler> _localizer;

    public GetBoardItemAttachmentByBoardIdRequestHandler(IRepository<BoardItemAttachment> repository, IStringLocalizer<GetBoardItemAttachmentByBoardIdRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<BoardItemAttachmentDto>> Handle(GetBoardItemAttachmentByBoardItemIdRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemAttachmentByBoardIdSpecification(request.MandantId, request.BoardItemId);
        var boardItemAttachments = await _repository.ListAsync(spec, cancellationToken);
        if (boardItemAttachments is null) throw new NotFoundException(_localizer["boarditemattachment.notfound"]);

        return boardItemAttachments.Adapt<List<BoardItemAttachmentDto>>();
    }
}

public class BoardItemAttachmentByBoardIdSpecification : Specification<BoardItemAttachment, BoardItemAttachmentDto>
{
    public BoardItemAttachmentByBoardIdSpecification(int mandantId, int boardItemId) =>
        Query.Where(x => x.MandantId == mandantId && x.BoardItemId == boardItemId);
}