using FSH.WebApi.Domain.Boards;
using Mapster;

namespace FSH.WebApi.Application.Tellus.BoardItemSubs;
public class GetBoardItemSubByBoardIdRequest : IRequest<List<BoardItemSubDto>>
{
    public int MandantId { get; set; }
    public int BoardItemId { get; set; }
    public GetBoardItemSubByBoardIdRequest(int mandantId, int boardItemId)
    {
        MandantId = mandantId;
        BoardItemId = boardItemId;
    }
}

public class GetBoardItemSubByBoardIdRequestHandler : IRequestHandler<GetBoardItemSubByBoardIdRequest, List<BoardItemSubDto>>
{
    private readonly IRepository<BoardItemSub> _repository;
    private readonly IStringLocalizer<GetBoardItemSubByBoardIdRequestHandler> _localizer;

    public GetBoardItemSubByBoardIdRequestHandler(IRepository<BoardItemSub> repository, IStringLocalizer<GetBoardItemSubByBoardIdRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<BoardItemSubDto>> Handle(GetBoardItemSubByBoardIdRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemSubByBoardIdSpecification(request.MandantId, request.BoardItemId);
        var boardItemSubs = await _repository.ListAsync(spec, cancellationToken);
        if (boardItemSubs is null) throw new NotFoundException(_localizer["boarditemsub.notfound"]);

        return boardItemSubs.Adapt<List<BoardItemSubDto>>();
    }
}

public class BoardItemSubByBoardIdSpecification : Specification<BoardItemSub, BoardItemSubDto>
{
    public BoardItemSubByBoardIdSpecification(int mandantId, int boardItemId) =>
        Query.Where(x => x.MandantId == mandantId && x.BoardItemId == boardItemId);
}