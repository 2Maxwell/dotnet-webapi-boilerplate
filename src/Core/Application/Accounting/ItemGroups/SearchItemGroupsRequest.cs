namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class SearchItemGroupsRequest : PaginationFilter, IRequest<PaginationResponse<ItemGroupDto>>
{
}

public class ItemGroupsBySearchRequestSpec : EntitiesByPaginationFilterSpec<ItemGroup, ItemGroupDto>
{
    public ItemGroupsBySearchRequestSpec(SearchItemGroupsRequest request)
        : base(request) =>
        Query
        .Where(c => c.MandantId == request.MandantId || c.MandantId == 0)
        .OrderBy(c => c.OrderNumber, !request.HasOrderBy());
}

public class SearchItemGroupsRequestHandler : IRequestHandler<SearchItemGroupsRequest, PaginationResponse<ItemGroupDto>>
{
    private readonly IReadRepository<ItemGroup> _repository;

    public SearchItemGroupsRequestHandler(IReadRepository<ItemGroup> repository) => _repository = repository;

    public async Task<PaginationResponse<ItemGroupDto>> Handle(SearchItemGroupsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ItemGroupsBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<ItemGroupDto>(list, count, request.PageNumber, request.PageSize);
    }
}