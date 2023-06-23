using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.Items;

public class UpdateItemRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public int ItemNumber { get; set; }
    public int ItemGroupId { get; set; }
    public bool Automatic { get; set; }
    public int InvoicePosition { get; set; }
    public int AccountNumber { get; set; }
    public List<ItemPriceTaxDto>? PriceTaxesDto { get; set; }
}

public class UpdateItemRequestHandler : IRequestHandler<UpdateItemRequest, int>
{
    private readonly IRepository<Item> _repository;
    private readonly IRepository<ItemPriceTax> _priceTaxRepository;
    private readonly IStringLocalizer<UpdateItemRequestHandler> _localizer;

    public UpdateItemRequestHandler(IRepository<Item> repository, IRepository<ItemPriceTax> priceTaxRepository, IStringLocalizer<UpdateItemRequestHandler> localizer)
    {
        _repository = repository;
        _priceTaxRepository = priceTaxRepository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateItemRequest request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = item ?? throw new NotFoundException(string.Format(_localizer["Item.notfound"], request.Id));

        var updatedItem = item.Update(
            request.Name!,
            request.ItemNumber,
            request.ItemGroupId,
            request.Automatic,
            request.InvoicePosition,
            request.AccountNumber);

        // Add Domain Events to be raised after the commit
        item.DomainEvents.Add(EntityUpdatedEvent.WithEntity(item));
        await _repository.UpdateAsync(updatedItem, cancellationToken);

        if (request.PriceTaxesDto is not null && request.PriceTaxesDto.Count() > 0)
        {
            foreach (var priceTax in request.PriceTaxesDto)
            {
                var existingPriceTax = await _priceTaxRepository.GetByIdAsync(priceTax.Id, cancellationToken);
                if (existingPriceTax is not null)
                {
                    var updatedPriceTax = existingPriceTax.Update(priceTax.Price, priceTax.TaxId, priceTax.Start, priceTax.End);
                    existingPriceTax.DomainEvents.Add(EntityUpdatedEvent.WithEntity(existingPriceTax));
                    await _priceTaxRepository.UpdateAsync(updatedPriceTax, cancellationToken);
                }
                else
                {
                    var newPriceTax = new ItemPriceTax(request.MandantId, item.Id, priceTax.Price, priceTax.TaxId, priceTax.Start, priceTax.End);
                    newPriceTax.DomainEvents.Add(EntityCreatedEvent.WithEntity(newPriceTax));
                    await _priceTaxRepository.AddAsync(newPriceTax, cancellationToken);
                }
            }
        }

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