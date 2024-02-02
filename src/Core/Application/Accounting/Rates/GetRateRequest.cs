using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Rates;

public class GetRateRequest : IRequest<RateDto>
{
    public int Id { get; set; }
    public GetRateRequest(int id) => Id = id;
}

public class GetRateRequestHandler : IRequestHandler<GetRateRequest, RateDto>
{
    private readonly IRepository<Rate> _repository;
    private readonly IStringLocalizer<GetRateRequestHandler> _localizer;

    public GetRateRequestHandler(IRepository<Rate> repository, IStringLocalizer<GetRateRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<RateDto> Handle(GetRateRequest request, CancellationToken cancellationToken)
    {
        RateDto? rateDto = await _repository.GetBySpecAsync(
            (ISpecification<Rate, RateDto>)new RateByIdSpec(request.Id), cancellationToken);

        if (rateDto == null) throw new NotFoundException(string.Format(_localizer["rate.notfound"], request.Id));

        return rateDto;
    }
}

public class RateByIdSpec : Specification<Rate, RateDto>, ISingleResultSpecification
{
    public RateByIdSpec(int id) => Query.Where(x => x.Id == id);
}
