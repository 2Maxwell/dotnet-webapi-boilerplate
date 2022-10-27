using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Zzzs;

public class SearchZzzsRequest : PaginationFilter, IRequest<PaginationResponse<ZzzDto>>
{
}

public class ZzzsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Zzz, ZzzDto>
{
    public ZzzsBySearchRequestSpec(SearchZzzsRequest request)
        : base(request) =>
        Query
        .Where(x => x.MandantId == request.MandantId)
        .OrderBy(x => x.Name, !request.HasOrderBy());
}

public class SearchZzzsRequestHandler : IRequestHandler<SearchZzzsRequest, PaginationResponse<ZzzDto>>
{
    private readonly IReadRepository<Zzz> _repository;

    public SearchZzzsRequestHandler(IReadRepository<Zzz> repository) =>
        _repository = repository;

    public async Task<PaginationResponse<ZzzDto>> Handle(SearchZzzsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ZzzsBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<ZzzDto>(list, count, request.PageNumber, request.PageSize);
    }
}
