using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Taxes;
public class UpdateTaxRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int CountryId { get; set; }
    public string? Name { get; set; }
    public int TaxSystemEnumId { get; set; }
    public List<TaxItemDto>? TaxItems { get; set; } = new();
}

public class UpdateTaxRequestValidator : CustomValidator<UpdateTaxRequest>
{
    public UpdateTaxRequestValidator(IReadRepository<Tax> repository, IStringLocalizer<UpdateTaxRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (tax, name, ct) =>
                await repository.GetBySpecAsync(new TaxByNameSpec(name, tax.MandantId), ct)
                is not Tax existing || existing.Id == tax.Id)
                .WithMessage((_, name) => string.Format(localizer["taxName.alreadyexists"], name));
    }
}

public class UpdateTaxRequestHandler : IRequestHandler<UpdateTaxRequest, int>
{
    private readonly IRepositoryWithEvents<Tax> _repository;
    private readonly IStringLocalizer _t;
    private readonly IRepositoryWithEvents<TaxItem> _taxItemRepository;

    public UpdateTaxRequestHandler(IRepositoryWithEvents<Tax> repository, IStringLocalizer<UpdateTaxRequestHandler> localizer, IRepositoryWithEvents<TaxItem> taxItemRepository)
    {
        _repository = repository;
        _t = localizer;
        _taxItemRepository = taxItemRepository;
    }

    public async Task<int> Handle(UpdateTaxRequest request, CancellationToken cancellationToken)
    {
        var tax = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = tax ?? throw new NotFoundException(string.Format(_t["tax.notfound"], request.Id));
        tax.Update(
            request.CountryId,
            request.Name,
            request.TaxSystemEnumId);            

        tax.DomainEvents.Add(EntityUpdatedEvent.WithEntity(tax));
        await _repository.UpdateAsync(tax, cancellationToken);

        if (request.TaxItems != null && request.TaxItems.Count() > 0)
        {
            foreach (TaxItemDto taxItemDto in request.TaxItems)
            {
                var taxItem = await _taxItemRepository.GetByIdAsync(taxItemDto.Id, cancellationToken);
                if (taxItem != null)
                {
                    // Update TaxItem
                    taxItem.Start = (DateTime)taxItemDto.Start;
                    taxItem.End = taxItemDto.End;
                    taxItem.TaxRate = taxItemDto.TaxRate;
                    taxItem.DomainEvents.Add(EntityCreatedEvent.WithEntity(taxItem));
                    await _taxItemRepository.UpdateAsync(taxItem, cancellationToken);
                }
                else
                {
                    // Create TaxItem
                    TaxItem p = new TaxItem(
                            request.Id,
                            request.MandantId,
                            taxItemDto.Start,
                            taxItemDto.End,
                            taxItemDto.TaxRate);
                    p.DomainEvents.Add(EntityCreatedEvent.WithEntity(p));
                    await _taxItemRepository.AddAsync(p, cancellationToken);
                }
            }
        }

        return request.Id;
    }
}