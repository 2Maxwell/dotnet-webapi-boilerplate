using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.VipStates;
public class GetVipStatusSelectRequest : IRequest<List<VipStatusSelectDto>>
{
    public GetVipStatusSelectRequest(int mandantId)
    {
        MandantId = mandantId;
    }

    public int MandantId { get; set; }
}

public class VipStatusSelectByMandantIdSpec : Specification<VipStatus, VipStatusSelectDto>
{
    public VipStatusSelectByMandantIdSpec(int MandantId)
    {
        Query
        .Where(x => x.MandantId == MandantId)
        .OrderBy(x => x.Name);
    }
}

public class GetVipStatusSelectRequestHandler : IRequestHandler<GetVipStatusSelectRequest, List<VipStatusSelectDto>>
{
    private readonly IRepository<VipStatus> _repository;
    private readonly IStringLocalizer<GetVipStatusSelectRequestHandler> _localizer;

    public GetVipStatusSelectRequestHandler(IRepository<VipStatus> repository, IStringLocalizer<GetVipStatusSelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<VipStatusSelectDto>> Handle(GetVipStatusSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<VipStatus, VipStatusSelectDto>)new VipStatusSelectByMandantIdSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["VipStatusSelect.notfound"], request.MandantId));
}
