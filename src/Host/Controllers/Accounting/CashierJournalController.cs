using FSH.WebApi.Application.Accounting.CashierJournals;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class CashierJournalController : VersionedApiController
{
    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new CashierJournal entry.", "")]
    public Task<int> CreateAsync(CreateCashierJournalRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("getCashierJournalOpenDto")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get CashierJournalOpenDtos.", "")]
    public Task<List<CashierJournalDto>> GetCashierJournalOpenAsync(GetCashierJournalOpenRequest request)
    {
        return Mediator.Send(request);
    }
}
