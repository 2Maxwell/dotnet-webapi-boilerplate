using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.PluGroups;

public class UpdatePluGroupRequest : IRequest<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int OrderNumber { get; set; }
}

public class UpdatePluGroupRequestHandler : IRequestHandler<UpdatePluGroupRequest, int>
{
    private readonly IRepository<PluGroup> _repository;
    private readonly IStringLocalizer<UpdatePluGroupRequestHandler> _localizer;

    public UpdatePluGroupRequestHandler(IRepository<PluGroup> repository, IStringLocalizer<UpdatePluGroupRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdatePluGroupRequest request, CancellationToken cancellationToken)
    {
        var pluGroup = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = pluGroup ?? throw new NotFoundException(string.Format(_localizer["pluGroup.notfound"], request.Id));

        var updatedPluGroup = pluGroup.Update(request.Name, request.OrderNumber);

        // Add Domain Events to be raised after the commit
        pluGroup.DomainEvents.Add(EntityUpdatedEvent.WithEntity(pluGroup));

        await _repository.UpdateAsync(updatedPluGroup, cancellationToken);

        return request.Id;
    }
}
