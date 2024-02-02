using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.BoardItemLabels;
public class GetBoardItemLabelByGroupRequest : IRequest<List<BoardItemLabelDto>>
{
    public int MandantId { get; set; }
    public int BoardItemLabelGroupId { get; set; }
    public GetBoardItemLabelByGroupRequest(int mandantId, int boardItemLabelGroupId)
    {
        MandantId = mandantId;
        BoardItemLabelGroupId = boardItemLabelGroupId;
    }
}

public class GetBoardItemLabelByGroupRequestHandler : IRequestHandler<GetBoardItemLabelByGroupRequest, List<BoardItemLabelDto>>
{
    private readonly IRepository<BoardItemLabel> _repository;
    private readonly IStringLocalizer<GetBoardItemLabelByGroupRequestHandler> _localizer;

    public GetBoardItemLabelByGroupRequestHandler(IRepository<BoardItemLabel> repository, IStringLocalizer<GetBoardItemLabelByGroupRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<BoardItemLabelDto>> Handle(GetBoardItemLabelByGroupRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemLabelByGroupSpecification(request.MandantId, request.BoardItemLabelGroupId);
        var boardItemLabels = await _repository.ListAsync(spec, cancellationToken);
        if (boardItemLabels is null) throw new NotFoundException(_localizer["boarditemlabel.notfound"]);

        return boardItemLabels;
    }
}

public class BoardItemLabelByGroupSpecification : Specification<BoardItemLabel, BoardItemLabelDto>
{
    public BoardItemLabelByGroupSpecification(int mandantId, int boardItemLabelGroupId) =>
        Query.Where(x => x.MandantId == mandantId && x.BoardItemLabelGroupId == boardItemLabelGroupId);
}