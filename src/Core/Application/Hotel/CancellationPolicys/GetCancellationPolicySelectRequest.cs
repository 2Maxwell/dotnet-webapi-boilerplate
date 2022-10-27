using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.CancellationPolicys;

public class GetCancellationPolicySelectRequest : IRequest<List<CancellationPolicySelectDto>>
{
    public int MandantId { get; set; }
    public GetCancellationPolicySelectRequest(int mandantId) => MandantId = mandantId;
}

public class CancellationPolicyByMandantIdSpec : Specification<CancellationPolicy, CancellationPolicySelectDto>
{
    public CancellationPolicyByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId)
             .OrderBy(c => c.Priority);
    }
}

public class GetCancellationPolicySelectRequestHandler : IRequestHandler<GetCancellationPolicySelectRequest, List<CancellationPolicySelectDto>>
{
    private readonly IRepository<CancellationPolicy> _repository;
    private readonly IStringLocalizer<GetCancellationPolicySelectRequestHandler> _localizer;

    public GetCancellationPolicySelectRequestHandler(IRepository<CancellationPolicy> repository, IStringLocalizer<GetCancellationPolicySelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<CancellationPolicySelectDto>> Handle(GetCancellationPolicySelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<CancellationPolicy, CancellationPolicySelectDto>)new CancellationPolicyByMandantIdSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["CancellationPolicySelect.notfound"], request.MandantId));
}