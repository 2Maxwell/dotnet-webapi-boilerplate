namespace FSH.WebApi.Application.Accounting.Items;

public class SearchItemsRequest : PaginationFilter, IRequest<PaginationResponse<ItemDto>>
{
}

public class ItemsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Item, ItemDto>
{
    public ItemsBySearchRequestSpec(SearchItemsRequest request)
        : base(request) =>
        Query
        .Where(i => i.MandantId == request.MandantId || i.MandantId == 0)
        .OrderBy(i => i.ItemNumber, !request.HasOrderBy());
}

public class SearchItemsRequestHandler : IRequestHandler<SearchItemsRequest, PaginationResponse<ItemDto>>
{
    private readonly IReadRepository<Item> _repository;

    public SearchItemsRequestHandler(IReadRepository<Item> repository) => _repository = repository;

    public async Task<PaginationResponse<ItemDto>> Handle(SearchItemsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ItemsBySearchRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<ItemDto>(list, count, request.PageNumber, request.PageSize);
    }

}