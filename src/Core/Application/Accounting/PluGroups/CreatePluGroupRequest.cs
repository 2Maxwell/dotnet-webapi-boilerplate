using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.PluGroups;

public class CreatePluGroupRequest : IRequest<int>
{
    public string Name { get; set; } = default!;
    public int OrderNumber { get; set; }
}

public class CreatePluGroupRequestHandler : IRequestHandler<CreatePluGroupRequest, int>
{
    private readonly IRepository<PluGroup> _repository;

    public CreatePluGroupRequestHandler(IRepository<PluGroup> repository) =>
        _repository = repository;

    public async Task<int> Handle(CreatePluGroupRequest request, CancellationToken cancellationToken)
    {
        var pluGroup = new PluGroup(request.Name, request.OrderNumber);

        pluGroup.DomainEvents.Add(EntityCreatedEvent.WithEntity(pluGroup));

        await _repository.AddAsync(pluGroup, cancellationToken);

        return pluGroup.Id;
    }

}
