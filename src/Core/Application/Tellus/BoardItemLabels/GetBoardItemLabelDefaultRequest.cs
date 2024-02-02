using FSH.WebApi.Domain.Boards;
using Mapster;

namespace FSH.WebApi.Application.Tellus.BoardItemLabels;
public class GetBoardItemLabelDefaultRequest : IRequest<List<BoardItemLabelDto>>
{
    public int MandantId { get; set; }
    public string? BoardItemLabelIds { get; set; } // Trenner: |
    public GetBoardItemLabelDefaultRequest(int mandantId, string? boardItemLabelIds)
    {
        MandantId = mandantId;
        BoardItemLabelIds = boardItemLabelIds;
    }
}

public class GetBoardItemLabelDefaultRequestHandler : IRequestHandler<GetBoardItemLabelDefaultRequest, List<BoardItemLabelDto>>
{
    private readonly IRepository<BoardItemLabel> _repository;
    private readonly IStringLocalizer<GetBoardItemLabelDefaultRequestHandler> _localizer;

    public GetBoardItemLabelDefaultRequestHandler(IRepository<BoardItemLabel> repository, IStringLocalizer<GetBoardItemLabelDefaultRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<List<BoardItemLabelDto>> Handle(GetBoardItemLabelDefaultRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardItemLabelDefaultSpecification(request.MandantId);
        var boardItemLabels = await _repository.ListAsync(spec, cancellationToken);
        if (boardItemLabels is null) throw new NotFoundException(_localizer["boarditemlabel.notfound"]);

        if (request.BoardItemLabelIds is not null)
        {
            var boardItemLabelIds = request.BoardItemLabelIds.Split('|').Select(int.Parse).ToList();
            foreach (int boardItemLabelId in boardItemLabelIds)
            {
                var boardItemLabel = await _repository.GetByIdAsync(boardItemLabelId, cancellationToken); 
                if (boardItemLabel is null) throw new NotFoundException(string.Format(_localizer["boarditemlabel.notfound"], boardItemLabelId));
                boardItemLabels.Add(boardItemLabel.Adapt<BoardItemLabelDto>());
            }
        }

        return boardItemLabels;
    }
}

public class BoardItemLabelDefaultSpecification : Specification<BoardItemLabel, BoardItemLabelDto>, ISingleResultSpecification
{
    public BoardItemLabelDefaultSpecification(int mandantId) =>
        Query.Where(x => x.MandantId == mandantId && x.DefaultLabel);

}