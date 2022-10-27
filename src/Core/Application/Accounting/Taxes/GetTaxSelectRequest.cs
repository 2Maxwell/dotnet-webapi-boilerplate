using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Taxes;
public class GetTaxSelectRequest : IRequest<List<TaxSelectDto>>
{
    public int MandantId { get; set; }
    public GetTaxSelectRequest(int mandantId)
    {
        MandantId = mandantId;
    }
}

public class TaxSelectByMandantIdSpec : Specification<Tax, TaxSelectDto>
{
    public TaxSelectByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId);
    }
}

public class GetTaxSelectRequestHandler : IRequestHandler<GetTaxSelectRequest, List<TaxSelectDto>>
{
    private readonly IRepository<Tax> _repository;
    private readonly IStringLocalizer<GetTaxSelectRequestHandler> _localizer;

    public GetTaxSelectRequestHandler(IRepository<Tax> repository, IStringLocalizer<GetTaxSelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);
    public async Task<List<TaxSelectDto>> Handle(GetTaxSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Tax, TaxSelectDto>)new TaxSelectByMandantIdSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["TaxSelect.notfound"], request.MandantId));
}