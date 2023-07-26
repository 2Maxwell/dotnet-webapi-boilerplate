using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Mandants;
public class CreateMandantDetailRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Name1 { get; set; }
    public string? Name2 { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Zip { get; set; }
    public string? City { get; set; }
    public int? CountryId { get; set; }
    public int? StateRegionId { get; set; }
    public string? Telephone { get; set; }
    public string? Telefax { get; set; }
    public string? Mobil { get; set; }
    public string? Email { get; set; }
    public string? EmailInvoice { get; set; }
    public string? WebSite { get; set; }
    public int? LanguageId { get; set; }
    public string? BankName { get; set; }
    public string? IBAN { get; set; }
    public string? BIC { get; set; }
    public string? TaxId { get; set; }
    public string? UStId { get; set; }
    public string? Company { get; set; }
    public string? CompanyRegister { get; set; }
}

public class CreateMandantDetailRequestValidator : CustomValidator<CreateMandantDetailRequest>
{
    public CreateMandantDetailRequestValidator()
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.Name1)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Name2)
            .MaximumLength(100);
        RuleFor(x => x.Address1)
            .MaximumLength(100);
        RuleFor(x => x.Address2)
            .MaximumLength(100);
        RuleFor(x => x.Zip)
            .MaximumLength(12);
        RuleFor(x => x.City)
            .MaximumLength(70);
        RuleFor(x => x.Telephone)
            .MaximumLength(25);
        RuleFor(x => x.Telefax)
            .MaximumLength(25);
        RuleFor(x => x.Mobil)
            .MaximumLength(25);
        RuleFor(x => x.Email)
            .MaximumLength(70);
        RuleFor(x => x.EmailInvoice)
            .MaximumLength(70);
        RuleFor(x => x.WebSite)
            .MaximumLength(70);
        RuleFor(x => x.BankName)
            .MaximumLength(70);
        RuleFor(x => x.IBAN)
            .MaximumLength(22);
        RuleFor(x => x.BIC)
            .MaximumLength(11);
        RuleFor(x => x.TaxId)
            .MaximumLength(30);
        RuleFor(x => x.UStId)
            .MaximumLength(30);
        RuleFor(x => x.Company)
            .MaximumLength(100);
        RuleFor(x => x.CompanyRegister)
            .MaximumLength(30);
    }
}

public class CreateMandantDetailRequestHandler : IRequestHandler<CreateMandantDetailRequest, int>
{
    private readonly IRepository<MandantDetail> _mandantDetailRepository;

    public CreateMandantDetailRequestHandler(IRepository<MandantDetail> mandantDetailRepository)
    {
        _mandantDetailRepository = mandantDetailRepository;
    }

    public async Task<int> Handle(CreateMandantDetailRequest request, CancellationToken cancellationToken)
    {
        var mandantDetail = new MandantDetail(
            request.MandantId,
            request.Name1,
            request.Name2,
            request.Address1,
            request.Address2,
            request.Zip,
            request.City,
            request.CountryId,
            request.StateRegionId,
            request.Telephone,
            request.Telefax,
            request.Mobil,
            request.Email,
            request.EmailInvoice,
            request.WebSite,
            request.LanguageId,
            request.BankName,
            request.IBAN,
            request.BIC,
            request.TaxId,
            request.UStId,
            request.Company,
            request.CompanyRegister);

        // Add Domain Events
        mandantDetail.DomainEvents.Add(EntityCreatedEvent.WithEntity(mandantDetail));
        await _mandantDetailRepository.AddAsync(mandantDetail, cancellationToken);
        return mandantDetail.MandantId;
    }
}