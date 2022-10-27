using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class CreateItemGroupRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string Name { get; set; } = default!;
    public int OrderNumber { get; set; }
}

public class CreateItemGroupRequestHandler : IRequestHandler<CreateItemGroupRequest, int>
{
    private readonly IRepository<ItemGroup> _repository;

    public CreateItemGroupRequestHandler(IRepository<ItemGroup> repository) =>
        _repository = repository;

    public async Task<int> Handle(CreateItemGroupRequest request, CancellationToken cancellationToken)
    {
        var itemGroup = new ItemGroup(request.MandantId, request.Name, request.OrderNumber);

        itemGroup.DomainEvents.Add(EntityCreatedEvent.WithEntity(itemGroup));

        await _repository.AddAsync(itemGroup, cancellationToken);

        return itemGroup.Id;
    }
}