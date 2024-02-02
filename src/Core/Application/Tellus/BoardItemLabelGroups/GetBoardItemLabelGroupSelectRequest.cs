using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.BoardItemLabelGroups;
public class GetBoardItemLabelGroupSelectRequest : IRequest<List<BoardItemLabelGroupSelectDto>>
{
    public int MandantId { get; set; }
    public GetBoardItemLabelGroupSelectRequest(int mandantId)
    {
        MandantId = mandantId;
    }
}

public class GetBoardItemLabelGroupSelectRequestHandler : IRequestHandler<GetBoardItemLabelGroupSelectRequest, List<BoardItemLabelGroupSelectDto>>
{
    private readonly IRepository<BoardItemLabelGroup> _repository;
    private readonly IStringLocalizer<GetBoardItemLabelGroupSelectRequestHandler> _localizer;

    public GetBoardItemLabelGroupSelectRequestHandler(IRepository<BoardItemLabelGroup> repository, IStringLocalizer<GetBoardItemLabelGroupSelectRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<BoardItemLabelGroupSelectDto>> Handle(GetBoardItemLabelGroupSelectRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemLabelGroupSelectSpecification(request.MandantId);
        var boardItemLabelGroups = await _repository.ListAsync(spec, cancellationToken);
        if (boardItemLabelGroups is null) throw new NotFoundException(_localizer["boarditemlabelgroup.notfound"]);

        return boardItemLabelGroups;
    }
}

public class BoardItemLabelGroupSelectSpecification : Specification<BoardItemLabelGroup, BoardItemLabelGroupSelectDto>
{
    public BoardItemLabelGroupSelectSpecification(int mandantId) =>
        Query.Where(x => x.MandantId == mandantId);
}