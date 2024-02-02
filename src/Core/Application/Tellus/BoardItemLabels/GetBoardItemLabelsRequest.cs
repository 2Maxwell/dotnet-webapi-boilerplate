using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.BoardItemLabels;
public class GetBoardItemLabelsRequest : IRequest<List<BoardItemLabelDto>>
{
    public int MandantId { get; set; }
}

public class GetBoardItemLabelsRequestHandler : IRequestHandler<GetBoardItemLabelsRequest, List<BoardItemLabelDto>>
{
    private readonly IRepository<BoardItemLabel> _repository;
    private readonly IStringLocalizer<GetBoardItemLabelsRequestHandler> _localizer;

    public GetBoardItemLabelsRequestHandler(IRepository<BoardItemLabel> repository, IStringLocalizer<GetBoardItemLabelsRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<BoardItemLabelDto>> Handle(GetBoardItemLabelsRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemLabelSpecification(request.MandantId);
        var boardItemLabels = await _repository.ListAsync(spec, cancellationToken);
        if (boardItemLabels is null) throw new NotFoundException(_localizer["boarditemlabel.notfound"]);

        return boardItemLabels;
    }
}

internal class BoardItemLabelSpecification : Specification<BoardItemLabel, BoardItemLabelDto>
{
    public BoardItemLabelSpecification(int mandantId) =>
        Query.Where(x => x.MandantId == mandantId);
}