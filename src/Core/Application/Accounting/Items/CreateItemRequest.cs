using FSH.WebApi.Domain.Accounting;
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
    public int InvoicePosition { get; set; }
    public int AccountNumber { get; set; }
    public List<ItemPriceTaxDto>? PriceTaxesDto { get; set; }
}

public class CreateItemRequestHandler : IRequestHandler<CreateItemRequest, int>
{
    private readonly IRepository<Item> _repository;
    private readonly IRepository<ItemPriceTax> _priceTaxRepository;

    public CreateItemRequestHandler(IRepository<Item> repository, IRepository<ItemPriceTax> priceTaxRepository)
    {
        _repository = repository;
        _priceTaxRepository = priceTaxRepository;
    }

    public async Task<int> Handle(CreateItemRequest request, CancellationToken cancellationToken)
    {
        var item = new Item(request.MandantId, request.Name, request.ItemNumber, request.ItemGroupId, request.Automatic, request.InvoicePosition, request.AccountNumber);
        item.DomainEvents.Add(EntityCreatedEvent.WithEntity(item));
        await _repository.AddAsync(item, cancellationToken);

        //foreach (ItemPriceTaxDto ptItem in request.PriceTaxesDto!)
        //{
        //    ItemPriceTax pt = new ItemPriceTax(request.MandantId, item.Id, ptItem.Price, ptItem.TaxId, ptItem.Start, ptItem.End);
        //    pt.DomainEvents.Add(EntityCreatedEvent.WithEntity(pt));
        //    await _priceTaxRepository.AddAsync(pt, cancellationToken);
        //}

        return item.Id;
    }
}