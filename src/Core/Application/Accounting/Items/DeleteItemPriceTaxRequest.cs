using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.Items;
public class DeleteItemPriceTaxRequest : IRequest<int>
{
    public int Id { get; set; }
    public DeleteItemPriceTaxRequest(int id) => Id = id;
}

public class DeleteItemPriceTaxRequestHandler : IRequestHandler<DeleteItemPriceTaxRequest, int>
{
    private readonly IRepository<ItemPriceTax> _repository;
    private readonly IStringLocalizer _t;

    public DeleteItemPriceTaxRequestHandler(IRepository<ItemPriceTax> repository, IStringLocalizer<DeleteItemPriceTaxRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<int> Handle(DeleteItemPriceTaxRequest request, CancellationToken cancellationToken)
    {
        var itemPriceTax = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = itemPriceTax ?? throw new NotFoundException(_t["ItemPriceTax {0} Not Found.", request.Id]);
        itemPriceTax.DomainEvents.Add(EntityDeletedEvent.WithEntity(itemPriceTax));
        await _repository.DeleteAsync(itemPriceTax, cancellationToken);
        return request.Id;
    }
}
