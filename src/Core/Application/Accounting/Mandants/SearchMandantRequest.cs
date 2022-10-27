using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Mandants;

public class SearchMandantRequest : PaginationFilter, IRequest<PaginationResponse<MandantDto>>
{
}

public class MandantBySearchRequestSpec : EntitiesByPaginationFilterSpec<Mandant, MandantDto>
{
    public MandantBySearchRequestSpec(SearchMandantRequest request)
        : base(request) =>
        Query.OrderBy(m => m.Name, !request.HasOrderBy());
}

public class SearchMandantRequestHandler : IRequestHandler<SearchMandantRequest, PaginationResponse<MandantDto>>
{
    private readonly IReadRepository<Mandant> _repository;

    public SearchMandantRequestHandler(IReadRepository<Mandant> repository) => _repository = repository;

    public async Task<PaginationResponse<MandantDto>> Handle(SearchMandantRequest request, CancellationToken cancellationToken)
    {
        var spec = new MandantBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);
        return new PaginationResponse<MandantDto>(list, count, request.PageNumber, request.PageSize);
    }
}