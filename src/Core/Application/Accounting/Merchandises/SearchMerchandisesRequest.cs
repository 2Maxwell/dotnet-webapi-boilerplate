using FSH.WebApi.Application.Accounting.PluGroups;
using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Merchandises;

public class SearchMerchandisesRequest : PaginationFilter, IRequest<PaginationResponse<MerchandiseDto>>
{
}

public class MerchandisesBySearchRequestSpec : EntitiesByPaginationFilterSpec<Merchandise, MerchandiseDto>
{
    public MerchandisesBySearchRequestSpec(SearchMerchandisesRequest request)
        : base(request) =>
        Query.OrderBy(c => c.MerchandiseNumber, !request.HasOrderBy());
}

public class SearchMerchandisesRequestHandler : IRequestHandler<SearchMerchandisesRequest, PaginationResponse<MerchandiseDto>>
{
    private readonly IReadRepository<Merchandise> _repository;

    public SearchMerchandisesRequestHandler(IReadRepository<Merchandise> repository) => _repository = repository;

    public async Task<PaginationResponse<MerchandiseDto>> Handle(SearchMerchandisesRequest request, CancellationToken cancellationToken)
    {
        var spec = new MerchandisesBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<MerchandiseDto>(list, count, request.PageNumber, request.PageSize);
    }
}
