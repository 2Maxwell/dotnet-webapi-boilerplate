using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.CashierJournals;
public class GetCashierJournalOpenRequest : IRequest<List<CashierJournalDto>>
{
    public int MandantId { get; set; }
    public int CashierRegisterId { get; set; }
}

public class GetCashierJournalOpenRequestSpec : Specification<CashierJournal, CashierJournalDto>
{
    public GetCashierJournalOpenRequestSpec(GetCashierJournalOpenRequest request)
    {
        Query
            .Where(x => x.MandantId == request.MandantId
                && x.KasseId == request.CashierRegisterId
                && x.State == 1);
    }
}

public class GetCashierJournalOpenRequestHandler : IRequestHandler<GetCashierJournalOpenRequest, List<CashierJournalDto>>
{
    private readonly IRepository<CashierJournal> _repository;

    public GetCashierJournalOpenRequestHandler(IRepository<CashierJournal> repository)
    {
        _repository = repository;
    }

    public async Task<List<CashierJournalDto>> Handle(GetCashierJournalOpenRequest request, CancellationToken cancellationToken)
    {
        var spec = new GetCashierJournalOpenRequestSpec(request);
        var result = await _repository.ListAsync(spec, cancellationToken);
        return result;
    }
}
