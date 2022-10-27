namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class GetItemGroupSelectRequest : IRequest<List<ItemGroupDto>>
{
    public int MandantId { get; set; }
    public GetItemGroupSelectRequest(int mandantId) => MandantId = mandantId;
}

public class GetItemGroupSelectSpec : Specification<ItemGroup, ItemGroupDto>
{
    public GetItemGroupSelectSpec(int mandantId) =>
        Query.Where(i => i.MandantId == 0 || i.MandantId == mandantId);
}

public class GetItemGroupSelectRequestHandler : IRequestHandler<GetItemGroupSelectRequest, List<ItemGroupDto>>
{
    private readonly IRepository<ItemGroup> _repository;
    private readonly IStringLocalizer<GetItemGroupSelectRequestHandler> _localizer;

    public GetItemGroupSelectRequestHandler(IRepository<ItemGroup> repository, IStringLocalizer<GetItemGroupSelectRequestHandler> localizer) => (_repository, _localizer) = (repository, localizer);

    public async Task<List<ItemGroupDto>> Handle(GetItemGroupSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<ItemGroup, ItemGroupDto>)new ItemGroupByMandantIdSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["ItemGroups.notfound"], request.MandantId));
}