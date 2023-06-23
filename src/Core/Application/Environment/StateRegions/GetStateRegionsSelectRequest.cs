using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.StateRegions;
public class GetStateRegionsSelectRequest : IRequest<List<StateRegionSelectDto>>
{
    public GetStateRegionsSelectRequest(int countryId)
    {
        CountryId = countryId;
    }

    public int CountryId { get; set; }
}

public class StateRegionsByCountryIdSpec : Specification<StateRegion, StateRegionSelectDto>
{
    public StateRegionsByCountryIdSpec(GetStateRegionsSelectRequest request)
    {
        if(request.CountryId == 0)
        {
            Query
                .OrderBy(x => x.Name);
        }
        else
        {
            Query
                .Where(x => x.CountryId == request.CountryId)
                .OrderBy(x => x.Name);
        }
    }
}

public class GetStateRegionsSelectRequestHandler : IRequestHandler<GetStateRegionsSelectRequest, List<StateRegionSelectDto>>
{
    private readonly IRepository<StateRegion> _repository;
    private readonly IStringLocalizer<GetStateRegionsSelectRequestHandler> _localizer;

    public GetStateRegionsSelectRequestHandler(IRepository<StateRegion> repository, IStringLocalizer<GetStateRegionsSelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<StateRegionSelectDto>> Handle(GetStateRegionsSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<StateRegion, StateRegionSelectDto>)new StateRegionsByCountryIdSpec(request), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["stateRegionSelect.notfound"]));
}