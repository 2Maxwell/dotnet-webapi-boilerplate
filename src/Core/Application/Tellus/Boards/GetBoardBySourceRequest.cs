using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.Boards;
public class GetBoardBySourceRequest : IRequest<BoardDto>
{
    public GetBoardBySourceRequest(string? source, int? sourceId, int mandantId)
    {
        Source = source;
        SourceId = sourceId;
        MandantId = mandantId;
    }

    public string? Source { get; set; }
    public int? SourceId { get; set; }
    public int MandantId { get; set; }
}

public class GetBoardBySourceRequestHandler : IRequestHandler<GetBoardBySourceRequest, BoardDto>
{
    private readonly IRepository<Board> _repository;
    private readonly IStringLocalizer<GetBoardBySourceRequestHandler> _localizer;

    public GetBoardBySourceRequestHandler(IRepository<Board> repository, IStringLocalizer<GetBoardBySourceRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<BoardDto> Handle(GetBoardBySourceRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardBySourceSpec(request.Source, request.SourceId, request.MandantId);
        BoardDto? board = new BoardDto();
        var boardDto = await _repository.GetBySpecAsync(spec, cancellationToken);
        if (boardDto is not null)
        {
            board = boardDto;
        }
        //await _repository.GetBySpecAsync((Specification<Board, BoardDto>)new BoardBySourceSpec(request.Source, request.SourceId, request.MandantId), cancellationToken)
        //?? throw new NotFoundException(string.Format(_localizer["board.notfound"], request.Source, request.SourceId));
        return board;
    }
}

internal class BoardBySourceSpec : Specification<Board, BoardDto>
{
    public BoardBySourceSpec(string? source, int? sourceId, int mandantId)
    {
        Query.Where(x => x.Source == source && x.SourceId == sourceId && x.MandantId == mandantId);
    }
}