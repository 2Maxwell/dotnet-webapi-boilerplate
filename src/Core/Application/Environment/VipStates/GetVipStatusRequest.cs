using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.VipStates;
public class GetVipStatusRequest : IRequest<VipStatusDto>
{
    public int Id { get; set; }
    public GetVipStatusRequest(int id) => Id = id;
}

public class GetVipStatusRequestHandler : IRequestHandler<GetVipStatusRequest, VipStatusDto>
{
    private readonly IRepository<VipStatus> _repository;
    private readonly IStringLocalizer<GetVipStatusRequestHandler> _localizer;

    public GetVipStatusRequestHandler(IRepository<VipStatus> repository, IStringLocalizer<GetVipStatusRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<VipStatusDto> Handle(GetVipStatusRequest request, CancellationToken cancellationToken)
    {
        VipStatusDto? vipStatusDto = await _repository.GetBySpecAsync(
            (ISpecification<VipStatus, VipStatusDto>)new VipStatusByIdSpec(request.Id), cancellationToken);

        // ?? throw new NotFoundException(string.Format(_localizer["vipStatus.notfound"], request.Id));

        if (vipStatusDto == null) throw new NotFoundException(string.Format(_localizer["vipStatus.notfound"], request.Id));

        return vipStatusDto;
    }
}

public class VipStatusByIdSpec : Specification<VipStatus, VipStatusDto>, ISingleResultSpecification
{
    public VipStatusByIdSpec(int id) => Query.Where(x => x.Id == id);
}
