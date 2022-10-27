using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Taxes;
public class CreateTaxRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public int CountryId { get; set; }
    public string? Name { get; set; }
    public int TaxSystemEnumId { get; set; }
    public List<TaxItemDto>? TaxItems { get; set; }
}

public class CreateTaxRequestValidator : CustomValidator<CreateTaxRequest>
{
    public CreateTaxRequestValidator(IReadRepository<Tax> repository, IStringLocalizer<CreateTaxRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (tax, name, ct) => await repository.GetBySpecAsync(new TaxByNameSpec(name, tax.MandantId), ct) is null)
            .WithMessage((_, name) => string.Format(localizer["taxName.alreadyexists"], name));
    }
}

public class TaxByNameSpec : Specification<Tax>, ISingleResultSpecification
{
    public TaxByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CreateTaxRequestHandler : IRequestHandler<CreateTaxRequest, int>
{
    private readonly IRepository<Tax> _repository;
    private readonly IRepository<TaxItem> _taxItemRepository;
    public CreateTaxRequestHandler(IRepository<Tax> repository, IRepository<TaxItem> taxItemRepository)
    {
        _repository = repository;
        _taxItemRepository = taxItemRepository;
    }

    public async Task<int> Handle(CreateTaxRequest request, CancellationToken cancellationToken)
    {
        var tax = new Tax(
            request.MandantId,
            request.CountryId,
            request.Name,
            request.TaxSystemEnumId);
        tax.DomainEvents.Add(EntityCreatedEvent.WithEntity(tax));
        await _repository.AddAsync(tax, cancellationToken);

        if (request.TaxItems != null && request.TaxItems.Count() > 0)
        {
            foreach (TaxItemDto taxItemDto in request.TaxItems)
            {
                TaxItem p = new TaxItem(
                    tax.Id,
                    tax.MandantId,
                    taxItemDto.Start,
                    taxItemDto.End,
                    taxItemDto.TaxRate);
                p.DomainEvents.Add(EntityCreatedEvent.WithEntity(p));
                await _taxItemRepository.AddAsync(p, cancellationToken);
            }
        }

        return tax.Id;
    }
}