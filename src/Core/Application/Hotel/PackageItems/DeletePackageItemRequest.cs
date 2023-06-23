using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PackageItems;

public class DeletePackageItemRequest : IRequest<int>
{
    public int Id { get; set; }
    public DeletePackageItemRequest(int id) => Id = id;
}

public class DeletePackageItemRequestHandler : IRequestHandler<DeletePackageItemRequest, int>
{
    private readonly IRepository<PackageItem> _repository;
    private readonly IStringLocalizer _t;

    public DeletePackageItemRequestHandler(IRepository<PackageItem> repository, IStringLocalizer<DeletePackageItemRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<int> Handle(DeletePackageItemRequest request, CancellationToken cancellationToken)
    {
        var packageItem = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = packageItem ?? throw new NotFoundException(_t["PackageItem {0} Not Found.", request.Id]);
        packageItem.DomainEvents.Add(EntityDeletedEvent.WithEntity(packageItem));
        await _repository.DeleteAsync(packageItem, cancellationToken);
        return request.Id;
    }
}
