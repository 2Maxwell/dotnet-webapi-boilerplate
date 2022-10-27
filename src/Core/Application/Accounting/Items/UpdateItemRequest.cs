using FSH.WebApi.Application.Accounting.ItemGroups;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Items;

public class UpdateItemRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string Name { get; set; }
    public int ItemNumber { get; set; }
    public int TaxId { get; set; }
    public decimal Price { get; set; }
    public int ItemGroupId { get; set; }
    public bool Automatic { get; set; }
}

public class UpdateItemRequestHandler : IRequestHandler<UpdateItemRequest, int>
{
    private readonly IRepository<Item> _repository;
    private readonly IStringLocalizer<UpdateItemRequestHandler> _localizer;

    public UpdateItemRequestHandler(IRepository<Item> repository, IStringLocalizer<UpdateItemRequestHandler> localizer) =>
    (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateItemRequest request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = item ?? throw new NotFoundException(string.Format(_localizer["Item.notfound"], request.Id));

        var updatedItem = item.Update(
            request.Name,
            request.ItemNumber,
            request.TaxId,
            request.Price,
            request.ItemGroupId,
            request.Automatic
            );

        // Add Domain Events to be raised after the commit
        item.DomainEvents.Add(EntityUpdatedEvent.WithEntity(item));

        await _repository.UpdateAsync(updatedItem, cancellationToken);

        return request.Id;
    }
}

public class UpdateItemRequestValidator : CustomValidator<UpdateItemRequest>
{
    public UpdateItemRequestValidator(IReadRepository<Item> repository, IStringLocalizer<UpdateItemRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(30)
            .MustAsync(async (item, name, ct) =>
                await repository.GetBySpecAsync(new ItemByNameSpec(name, item.MandantId), ct)
                is not Item existingItem || existingItem.MandantId == item.MandantId)
                .WithMessage((_, name) => localizer["Item {0} already Exists.", name]);

        RuleFor(p => p.ItemNumber)
            .NotEmpty()
            .GreaterThan(999)
            .LessThan(100000)
            .MustAsync(async (item, itemNumber, ct) =>
                await repository.GetBySpecAsync(new ItemByItemNumberSpec(itemNumber, item.MandantId), ct)
                is not Item existingItem || existingItem.MandantId == item.MandantId)
                .WithMessage((_, itemNumber) => localizer["Item {0} already Exists.", itemNumber]);

    }
}