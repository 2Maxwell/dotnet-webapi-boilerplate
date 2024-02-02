using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.Boards;
public class SearchBoardsRequest : PaginationFilter, IRequest<PaginationResponse<BoardDto>>
{
    public string? Title { get; set; }
}

public class SearchBoardsRequestHandler : IRequestHandler<SearchBoardsRequest, PaginationResponse<BoardDto>>
{
    private readonly IReadRepository<Board> _repository;

    public SearchBoardsRequestHandler(IReadRepository<Board> repository) => _repository = repository;

    public async Task<PaginationResponse<BoardDto>> Handle(SearchBoardsRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardsBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}

public class BoardsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Board, BoardDto>
{
    public BoardsBySearchRequestSpec(SearchBoardsRequest request)
        :base(request) =>
        Query
            .OrderBy(x => x.Title, !request.HasOrderBy())
            .Where(x => x.MandantId == request.MandantId)
            .Where(x => x.Title.Contains(request.Title!), !string.IsNullOrEmpty(request.Title));
}