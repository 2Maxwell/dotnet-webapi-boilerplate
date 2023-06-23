namespace FSH.WebApi.Application.Accounting.Items;

public class GetItemSelectRequest : IRequest<List<ItemSelectDto>>
{
    public int MandantId { get; set; }
    public GetItemSelectRequest(int mandantId) => MandantId = mandantId;
}

public class GetItemSelectRequestHandler : IRequestHandler<GetItemSelectRequest, List<ItemSelectDto>>
{
    private readonly IRepository<Item> _repository;
    private readonly IStringLocalizer<GetItemSelectRequestHandler> _localizer;

    public GetItemSelectRequestHandler(IRepository<Item> repository, IStringLocalizer<GetItemSelectRequestHandler> localizer) => (_repository, _localizer) = (repository, localizer);

    public async Task<List<ItemSelectDto>> Handle(GetItemSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Item, ItemSelectDto>)new ItemByMandantIdAndMandantId0Spec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["Items.notfound"], request.MandantId));
}

public class ItemByMandantIdAndMandantId0Spec : Specification<Item, ItemSelectDto>
{
    public ItemByMandantIdAndMandantId0Spec(int mandantId) =>
        Query.Where(i => i.MandantId == mandantId || i.MandantId == 0);
}

public class ItemsByMandantIdAndMandantId0Spec : Specification<Item, ItemDto>
{
    public ItemsByMandantIdAndMandantId0Spec(int mandantId) =>
        Query
        .Where(i => i.MandantId == mandantId || i.MandantId == 0);
}