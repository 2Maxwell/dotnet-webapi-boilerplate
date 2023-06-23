using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.VipStates;
public class SearchVipStatesRequest : PaginationFilter, IRequest<PaginationResponse<VipStatusDto>>
{
}

public class VipStatesByMandantIdSearchRequestSpec : EntitiesByPaginationFilterSpec<VipStatus, VipStatusDto>
{
    public VipStatesByMandantIdSearchRequestSpec(SearchVipStatesRequest request)
        : base(request) =>
        Query
        .Where(x => x.MandantId == request.MandantId )
        .OrderBy(x => x.Name, !request.HasOrderBy());
}

public class SearchVipStatesRequestHandler : IRequestHandler<SearchVipStatesRequest, PaginationResponse<VipStatusDto>>
{
    private readonly IReadRepository<VipStatus> _repository;

    public SearchVipStatesRequestHandler(IReadRepository<VipStatus> repository) =>
        _repository = repository;

    public async Task<PaginationResponse<VipStatusDto>> Handle(SearchVipStatesRequest request, CancellationToken cancellationToken)
    {
        var spec = new VipStatesByMandantIdSearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<VipStatusDto>(list, count, request.PageNumber, request.PageSize);
    }
}