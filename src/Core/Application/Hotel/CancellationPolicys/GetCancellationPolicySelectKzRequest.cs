using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.CancellationPolicys;

public class GetCancellationPolicySelectKzRequest : IRequest<List<CancellationPolicySelectKzDto>>
{
    public int MandantId { get; set; }
    public GetCancellationPolicySelectKzRequest(int mandantId) => MandantId = mandantId;
}

public class CancellationPolicySelectKzByMandantIdSpec : Specification<CancellationPolicy, CancellationPolicySelectKzDto>
{
    public CancellationPolicySelectKzByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId)
             .OrderBy(c => c.Priority);
    }
}

public class GetCancellationPolicySelectKzRequestHandler : IRequestHandler<GetCancellationPolicySelectKzRequest, List<CancellationPolicySelectKzDto>>
{
    private readonly IRepository<CancellationPolicy> _repository;
    private readonly IStringLocalizer<GetCancellationPolicySelectKzRequestHandler> _localizer;

    public GetCancellationPolicySelectKzRequestHandler(IRepository<CancellationPolicy> repository, IStringLocalizer<GetCancellationPolicySelectKzRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<CancellationPolicySelectKzDto>> Handle(GetCancellationPolicySelectKzRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<CancellationPolicy, CancellationPolicySelectKzDto>)new CancellationPolicySelectKzByMandantIdSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["CancellationPolicySelectKz.notfound"], request.MandantId));
}

