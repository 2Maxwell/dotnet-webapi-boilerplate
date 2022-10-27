using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.CancellationPolicys;

public class SearchCancellationPolicysRequest : PaginationFilter, IRequest<PaginationResponse<CancellationPolicyDto>>
{
}

public class CancellationPolicysBySearchRequestSpec : EntitiesByPaginationFilterSpec<CancellationPolicy, CancellationPolicyDto>
{
    public CancellationPolicysBySearchRequestSpec(SearchCancellationPolicysRequest request)
        : base(request) =>
        Query
        .OrderBy(p => p.Name, !request.HasOrderBy());
}

public class SearchCancellationPolicysRequestHandler : IRequestHandler<SearchCancellationPolicysRequest, PaginationResponse<CancellationPolicyDto>>
{
    private readonly IReadRepository<CancellationPolicy> _repository;

    public SearchCancellationPolicysRequestHandler(IReadRepository<CancellationPolicy> repository) =>
        _repository = repository;

    public async Task<PaginationResponse<CancellationPolicyDto>> Handle(SearchCancellationPolicysRequest request, CancellationToken cancellationToken)
    {
        var spec = new CancellationPolicysBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<CancellationPolicyDto>(list, count, request.PageNumber, request.PageSize);
    }
}
