using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Items;

public class SearchItemsRequest : PaginationFilter, IRequest<PaginationResponse<ItemDto>>
{
}

public class ItemsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Item, ItemDto>
{
    public ItemsBySearchRequestSpec(SearchItemsRequest request)
        : base(request) =>
        Query
        .Where(i => i.MandantId == request.MandantId || i.MandantId == 0)
        .OrderBy(i => i.ItemNumber, !request.HasOrderBy());
}

public class SearchItemsRequestHandler : IRequestHandler<SearchItemsRequest, PaginationResponse<ItemDto>>
{
    private readonly IReadRepository<Item> _repository;
    private readonly IReadRepository<ItemPriceTax> _priceTaxRepository;

    public SearchItemsRequestHandler(IReadRepository<Item> repository, IReadRepository<ItemPriceTax> priceTaxRepository)
    {
        _repository = repository;
        _priceTaxRepository = priceTaxRepository;
    }

    public async Task<PaginationResponse<ItemDto>> Handle(SearchItemsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ItemsBySearchRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);

        foreach (var item in list)
        {
            // lade PriceTaxes.Dto
            item.PriceTaxesDto = await _priceTaxRepository.ListAsync(
                (ISpecification<ItemPriceTax, ItemPriceTaxDto>)new ItemPriceTaxByItemIdSpec(item.Id), cancellationToken);

        }

        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<ItemDto>(list, count, request.PageNumber, request.PageSize);
    }

}