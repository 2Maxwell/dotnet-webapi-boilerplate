using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Items;

public class GetItemRequest : IRequest<ItemDto>
{
    public int Id { get; set; }
    public GetItemRequest(int id) => Id = id;
}

public class GetItemRequestHandler : IRequestHandler<GetItemRequest, ItemDto>
{
    private readonly IRepository<Item> _repository;
    private readonly IRepository<ItemPriceTax> _priceTaxRepository;
    private readonly IStringLocalizer<GetItemRequestHandler> _localizer;

    public GetItemRequestHandler(IRepository<Item> repository, IRepository<ItemPriceTax> priceTaxRepository, IStringLocalizer<GetItemRequestHandler> localizer)
    {
        _repository = repository;
        _priceTaxRepository = priceTaxRepository;
        _localizer = localizer;
    }

    public async Task<ItemDto> Handle(GetItemRequest request, CancellationToken cancellationToken)
    {
        ItemDto? itemDto = await _repository.GetBySpecAsync(
            (ISpecification<Item, ItemDto>)new ItemByIdSpec(request.Id), cancellationToken);

        var priceTaxes = await _priceTaxRepository.ListAsync(
                   (ISpecification<ItemPriceTax, ItemPriceTaxDto>)new ItemPriceTaxByItemIdSpec(request.Id), cancellationToken);

        itemDto!.PriceTaxesDto = priceTaxes;

        return itemDto;
    }
}

public class ItemByIdSpec : Specification<Item, ItemDto>, ISingleResultSpecification
{
    public ItemByIdSpec(int id) => Query.Where(i => i.Id == id);
}

public class ItemPriceTaxByItemIdSpec : Specification<ItemPriceTax, ItemPriceTaxDto>
{
    public ItemPriceTaxByItemIdSpec(int id) => Query.Where(ipt => ipt.ItemId == id);
}
