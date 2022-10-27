using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Rates;

public class SearchRatesRequest : PaginationFilter, IRequest<PaginationResponse<RateDto>>
{
}

public class RatesBySearchRequestSpec : EntitiesByPaginationFilterSpec<Rate, RateDto>
{
    public RatesBySearchRequestSpec(SearchRatesRequest request)
        : base(request) =>
        Query
        .OrderBy(p => p.Name, !request.HasOrderBy());
}

public class SearchRatesRequestHandler : IRequestHandler<SearchRatesRequest, PaginationResponse<RateDto>>
{
    private readonly IReadRepository<Rate> _repository;

    public SearchRatesRequestHandler(IReadRepository<Rate> repository) =>
        _repository = repository;

    public async Task<PaginationResponse<RateDto>> Handle(SearchRatesRequest request, CancellationToken cancellationToken)
    {
        var spec = new RatesBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<RateDto>(list, count, request.PageNumber, request.PageSize);
    }
}
