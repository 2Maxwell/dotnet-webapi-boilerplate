using FSH.WebApi.Domain.Boards;

namespace FSH.WebApi.Application.Tellus.BoardCollections;
public class SearchBoardCollectionsRequest : PaginationFilter, IRequest<PaginationResponse<BoardCollectionDto>>
{
    public string? Title { get; set; }
}

public class SearchBoardCollectionsRequestHandler : IRequestHandler<SearchBoardCollectionsRequest, PaginationResponse<BoardCollectionDto>>
{
    private readonly IReadRepository<BoardCollection> _repository;

    public SearchBoardCollectionsRequestHandler(IReadRepository<BoardCollection> repository) => _repository = repository;

    public async Task<PaginationResponse<BoardCollectionDto>> Handle(SearchBoardCollectionsRequest request, CancellationToken cancellationToken)
    {
        var spec = new BoardCollectionsBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}

public class BoardCollectionsBySearchRequestSpec : EntitiesByPaginationFilterSpec<BoardCollection, BoardCollectionDto>
{
    public BoardCollectionsBySearchRequestSpec(SearchBoardCollectionsRequest request)
        : base(request) =>
        Query
            .OrderBy(x => x.Title, !request.HasOrderBy())
            .Where(x => x.MandantId == request.MandantId)
            .Where(x => x.Title!.Contains(request.Title!), !string.IsNullOrEmpty(request.Title));
}