namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class GetItemGroupRequest : IRequest<ItemGroupDto>
{
    public int Id { get; set; }
    public GetItemGroupRequest(int id) => Id = id;
}

public class ItemGroupByIdSpec : Specification<ItemGroup, ItemGroupDto>, ISingleResultSpecification
{
    public ItemGroupByIdSpec(int id) => Query.Where(i => i.Id == id);
}

public class GetItemGroupRequestHandler : IRequestHandler<GetItemGroupRequest, ItemGroupDto>
{
    private readonly IRepository<ItemGroup> _repository;
    private readonly IStringLocalizer<GetItemGroupRequestHandler> _localizer;

    public GetItemGroupRequestHandler(IRepository<ItemGroup> repository, IStringLocalizer<GetItemGroupRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<ItemGroupDto> Handle(GetItemGroupRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<ItemGroup, ItemGroupDto>)new ItemGroupByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["itemGroup.notfound"], request.Id));
}