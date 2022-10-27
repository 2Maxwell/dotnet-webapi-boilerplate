using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class UpdateItemGroupRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string Name { get; set; }
    public int OrderNumber { get; set; }
}

public class UpdateItemGroupRequestHandler : IRequestHandler<UpdateItemGroupRequest, int>
{
    private readonly IRepository<ItemGroup> _repository;
    private readonly IStringLocalizer<UpdateItemGroupRequestHandler> _localizer;

    public UpdateItemGroupRequestHandler(IRepository<ItemGroup> repository, IStringLocalizer<UpdateItemGroupRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateItemGroupRequest request, CancellationToken cancellationToken)
    {
        var itemGroup = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = itemGroup ?? throw new NotFoundException(string.Format(_localizer["ItemGroup.notfound"], request.Id));

        var updatedItemGroup = itemGroup.Update(request.Name, request.OrderNumber);

        // Add Domain Events to be raised after the commit
        itemGroup.DomainEvents.Add(EntityUpdatedEvent.WithEntity(itemGroup));

        await _repository.UpdateAsync(updatedItemGroup, cancellationToken);

        return request.Id;
    }
}

