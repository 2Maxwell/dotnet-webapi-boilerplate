using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Hotel.Periods;

public class DeletePeriodRequest : IRequest<int>
{
    public int Id { get; set; }
    public DeletePeriodRequest(int id) => Id = id;
}

public class DeletePeriodRequestHandler : IRequestHandler<DeletePeriodRequest, int>
{
    private readonly IRepository<Period> _repository;
    private readonly IStringLocalizer _t;

    public DeletePeriodRequestHandler(IRepository<Period> repository, IStringLocalizer<DeletePeriodRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<int> Handle(DeletePeriodRequest request, CancellationToken cancellationToken)
    {
        var period = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = period ?? throw new NotFoundException(_t["Period {0} Not Found."]);

        period.DomainEvents.Add(EntityDeletedEvent.WithEntity(period));

        await _repository.DeleteAsync(period, cancellationToken);
        return request.Id;
    }
}

