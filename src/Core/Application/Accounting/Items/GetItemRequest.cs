namespace FSH.WebApi.Application.Accounting.Items;

public class GetItemRequest : IRequest<ItemDto>
{
    public int Id { get; set; }
    public GetItemRequest(int id) => Id = id;
}

public class GetItemRequestHandler : IRequestHandler<GetItemRequest, ItemDto>
{
    private readonly IRepository<Item> _repository;
    private readonly IStringLocalizer<GetItemRequestHandler> _localizer;

    public GetItemRequestHandler(IRepository<Item> repository, IStringLocalizer<GetItemRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<ItemDto> Handle(GetItemRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Item, ItemDto>) new ItemByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["item.notfound"], request.Id));
}

public class ItemByIdSpec : Specification<Item, ItemDto>, ISingleResultSpecification
{
    public ItemByIdSpec(int id) => Query.Where(i => i.Id == id);
}