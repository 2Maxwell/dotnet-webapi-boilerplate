using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Taxes;
public class SearchTaxesRequest : PaginationFilter, IRequest<PaginationResponse<TaxDto>>
{
}

public class TaxesBySearchRequestSpec : EntitiesByPaginationFilterSpec<Tax, TaxDto>
{
    public TaxesBySearchRequestSpec(SearchTaxesRequest request)
        : base(request) =>
        Query
        .Include(p => p.TaxItems)
        .OrderBy(p => p.Name, !request.HasOrderBy());
}

public class SearchTaxesRequestHandler : IRequestHandler<SearchTaxesRequest, PaginationResponse<TaxDto>>
{
    private readonly IReadRepository<Tax> _repository;
    private readonly IRepository<TaxItem> _taxItemRepository;

    public SearchTaxesRequestHandler(IReadRepository<Tax> repository, IRepository<TaxItem> taxItemRepository)
    {
        _repository = repository;
        _taxItemRepository = taxItemRepository;
    }

    public async Task<PaginationResponse<TaxDto>> Handle(SearchTaxesRequest request, CancellationToken cancellationToken)
    {
        var spec = new TaxesBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<TaxDto>(list, count, request.PageNumber, request.PageSize);
    }
}
