using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.TaxItems;
public class DeleteTaxItemRequest : IRequest<int>
{
    public int Id { get; set; }
    public DeleteTaxItemRequest(int id) => Id = id;
}

public class DeleteTaxItemRequestHandler : IRequestHandler<DeleteTaxItemRequest, int>
{
    private readonly IRepository<TaxItem> _repository;
    private readonly IStringLocalizer _t;

    public DeleteTaxItemRequestHandler(IRepository<TaxItem> repository, IStringLocalizer<DeleteTaxItemRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<int> Handle(DeleteTaxItemRequest request, CancellationToken cancellationToken)
    {
        var taxItem = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = taxItem ?? throw new NotFoundException(_t["TaxItem {0} Not Found."]);
        taxItem.DomainEvents.Add(EntityDeletedEvent.WithEntity(taxItem));
        await _repository.DeleteAsync(taxItem, cancellationToken);
        return request.Id;
    }
}
