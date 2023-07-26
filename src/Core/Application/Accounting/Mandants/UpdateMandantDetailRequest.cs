using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Mandants;
public class UpdateMandantDetailRequest : IRequest<int>
{
    public int Id { get; set; }
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

public class UpdateMandantDetailRequestValidator : CustomValidator<UpdateMandantDetailRequest>
{
    public UpdateMandantDetailRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
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

public class UpdateMandantDetailRequestHandler : IRequestHandler<UpdateMandantDetailRequest, int>
{
    private readonly IRepository<MandantDetail> _repository;
    private readonly IStringLocalizer _localizer;

    public UpdateMandantDetailRequestHandler(IRepository<MandantDetail> repository, IStringLocalizer<UpdateMandantDetailRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateMandantDetailRequest request, CancellationToken cancellationToken)
    {
        var mandantDetail = await _repository.GetByIdAsync(request.Id);
        if (mandantDetail == null)
        {
            throw new NotFoundException(_localizer["MandantDetailNotFound"]);
        }

        mandantDetail.Name1 = request.Name1;
        mandantDetail.Name2 = request.Name2;
        mandantDetail.Address1 = request.Address1;
        mandantDetail.Address2 = request.Address2;
        mandantDetail.Zip = request.Zip;
        mandantDetail.City = request.City;
        mandantDetail.CountryId = request.CountryId;
        mandantDetail.StateRegionId = request.StateRegionId;
        mandantDetail.Telephone = request.Telephone;
        mandantDetail.Telefax = request.Telefax;
        mandantDetail.Mobil = request.Mobil;
        mandantDetail.Email = request.Email;
        mandantDetail.EmailInvoice = request.EmailInvoice;
        mandantDetail.WebSite = request.WebSite;
        mandantDetail.LanguageId = request.LanguageId;
        mandantDetail.BankName = request.BankName;
        mandantDetail.IBAN = request.IBAN;
        mandantDetail.BIC = request.BIC;
        mandantDetail.TaxId = request.TaxId;
        mandantDetail.UStId = request.UStId;
        mandantDetail.Company = request.Company;
        mandantDetail.CompanyRegister = request.CompanyRegister;

        await _repository.UpdateAsync(mandantDetail, cancellationToken);

        return mandantDetail.Id;
    }
}