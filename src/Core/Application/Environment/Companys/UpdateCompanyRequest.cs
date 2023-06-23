using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Companys;
public class UpdateCompanyRequest : IRequest<int>
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
    public int? CompanyTypEnumId { get; set; } // Company, TravelAgent, Portal
    public decimal? CommissionRate { get; set; }
    public bool? AddTax { get; set; }
    public int GroupHeadId { get; set; } // Id of CompanyHead
    public int? PriceContractId { get; set; }
    public string? Kz { get; set; }
    public int? StatusId { get; set; }
    public string? Text { get; set; }
}

public class UpdateCompanyRequestValidator : CustomValidator<UpdateCompanyRequest>
{
    public UpdateCompanyRequestValidator(IReadRepository<Company> repository, IStringLocalizer<UpdateCompanyRequestValidator> localizer)
    {
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
            .EmailAddress()
            .MaximumLength(70);
        RuleFor(x => x.EmailInvoice)
            .EmailAddress()
            .MaximumLength(70);
        RuleFor(x => x.WebSite)
            .MaximumLength(70);
        RuleFor(x => x.Kz)
            .MaximumLength(50);
        RuleFor(x => x.Text)
            .MaximumLength(150);
    }
}

public class UpdateCompanyRequestHandler : IRequestHandler<UpdateCompanyRequest, int>
{
    private readonly IRepositoryWithEvents<Company> _repository;
    private readonly IStringLocalizer<UpdateCompanyRequestHandler> _localizer;

    public UpdateCompanyRequestHandler(IRepositoryWithEvents<Company> repository, IStringLocalizer<UpdateCompanyRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = company ?? throw new NotFoundException(string.Format(_localizer["company.notfound"], request.Id));
        company.Update(
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
                    request.CompanyTypEnumId,
                    request.CommissionRate,
                    request.AddTax,
                    request.GroupHeadId,
                    request.PriceContractId,
                    request.Kz,
                    request.StatusId,
                    request.Text
                    );
        company.DomainEvents.Add(EntityUpdatedEvent.WithEntity(company));
        await _repository.UpdateAsync(company, cancellationToken);
        return request.Id;
    }

}