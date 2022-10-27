using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.PluGroups;

public class SearchPluGroupsRequest : PaginationFilter, IRequest<PaginationResponse<PluGroupDto>>
{
}

public class PluGroupsBySearchRequestSpec : EntitiesByPaginationFilterSpec<PluGroup, PluGroupDto>
{
    public PluGroupsBySearchRequestSpec(SearchPluGroupsRequest request)
        : base(request) =>
        Query.OrderBy(c => c.OrderNumber, !request.HasOrderBy());
}

public class SearchPluGroupsRequestHandler : IRequestHandler<SearchPluGroupsRequest, PaginationResponse<PluGroupDto>>
{
    private readonly IReadRepository<PluGroup> _repository;

    public SearchPluGroupsRequestHandler(IReadRepository<PluGroup> repository) => _repository = repository;

    public async Task<PaginationResponse<PluGroupDto>> Handle(SearchPluGroupsRequest request, CancellationToken cancellationToken)
    {
        var spec = new PluGroupsBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<PluGroupDto>(list, count, request.PageNumber, request.PageSize);
    }
}