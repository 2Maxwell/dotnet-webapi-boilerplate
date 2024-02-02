using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.CashierRegisters;
public class SearchCashierRegisterRequest : PaginationFilter, IRequest<PaginationResponse<CashierRegisterDto>>
{
    public SearchCashierRegisterRequest(int mandantId)
    {
        MandantId = mandantId;
    }

    public int MandantId { get; set; }
}

public class CashierRegisterSearchRequestSpec : EntitiesByPaginationFilterSpec<CashierRegister, CashierRegisterDto>
{
    public CashierRegisterSearchRequestSpec(SearchCashierRegisterRequest request)
        : base(request) =>
        Query
        .Where(c => c.MandantId == request.MandantId)
        .OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchCashierRegisterRequestHandler : IRequestHandler<SearchCashierRegisterRequest, PaginationResponse<CashierRegisterDto>>
{
    private readonly IReadRepository<CashierRegister> _repository;

    public SearchCashierRegisterRequestHandler(IReadRepository<CashierRegister> repository) => _repository = repository;

    public async Task<PaginationResponse<CashierRegisterDto>> Handle(SearchCashierRegisterRequest request, CancellationToken cancellationToken)
    {
        var spec = new CashierRegisterSearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<CashierRegisterDto>(list, count, request.PageNumber, request.PageSize);
    }
}
