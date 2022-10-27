using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.Items;

public class CreateItemRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string Name { get; set; } = default!;
    public int ItemNumber { get; set; }
    public int TaxId { get; set; }
    public decimal Price { get; set; }
    public int ItemGroupId { get; set; }
    public bool Automatic { get; set; }
}

public class CreateItemRequestHandler : IRequestHandler<CreateItemRequest, int>
{
    private readonly IRepository<Item> _repository;
    public CreateItemRequestHandler(IRepository<Item> repository) => _repository = repository;

    public async Task<int> Handle(CreateItemRequest request, CancellationToken cancellationToken)
    {
        var item = new Item(request.MandantId, request.Name, request.ItemNumber, request.TaxId, request.Price, request.ItemGroupId, request.Automatic);
        item.DomainEvents.Add(EntityCreatedEvent.WithEntity(item));
        await _repository.AddAsync(item, cancellationToken);

        return item.Id;
    }
}