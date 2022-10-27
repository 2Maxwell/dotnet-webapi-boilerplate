using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.CancellationPolicys;

public class GetCancellationPolicyRequest : IRequest<CancellationPolicyDto>
{
    public int Id { get; set; }
    public GetCancellationPolicyRequest(int id) => Id = id;
}

public class GetCancellationPolicyRequestHandler : IRequestHandler<GetCancellationPolicyRequest, CancellationPolicyDto>
{
    private readonly IRepository<CancellationPolicy> _repository;
    private readonly IStringLocalizer<GetCancellationPolicyRequestHandler> _localizer;

    public GetCancellationPolicyRequestHandler(IRepository<CancellationPolicy> repository, IStringLocalizer<GetCancellationPolicyRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<CancellationPolicyDto> Handle(GetCancellationPolicyRequest request, CancellationToken cancellationToken)
    {
        CancellationPolicyDto? cancellationPolicyDto = await _repository.GetBySpecAsync(
            (ISpecification<CancellationPolicy, CancellationPolicyDto>)new CancellationPolicyByIdSpec(request.Id), cancellationToken);

        // ?? throw new NotFoundException(string.Format(_localizer["cancellationPolicy.notfound"], request.Id));

        if (cancellationPolicyDto == null) throw new NotFoundException(string.Format(_localizer["cancellationPolicy.notfound"], request.Id));

        return cancellationPolicyDto;
    }
}

public class CancellationPolicyByIdSpec : Specification<CancellationPolicy, CancellationPolicyDto>, ISingleResultSpecification
{
    public CancellationPolicyByIdSpec(int id) => Query.Where(x => x.Id == id);
}
