using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Companys;
public class CreateCompanyRequest : IRequest<int>
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
    public int? CompanyTypEnumId { get; set; } // Company, TravelAgent, Portal
    public decimal? CommissionRate { get; set; }
    public bool? AddTax { get; set; }
    public int? GroupHeadId { get; set; } // Id of CompanyHead
    public int? PriceContractId { get; set; }
    public string? Kz { get; set; }
    public int? StatusId { get; set; }
    public string? Text { get; set; }
}

public class CreateCompanyRequestValidator : CustomValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator(IReadRepository<Company> repository, IStringLocalizer<CreateCompanyRequestValidator> localizer)
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

public class CreateCompanyRequestHandler : IRequestHandler<CreateCompanyRequest, int>
{
    private readonly IRepository<Company> _repository;

    public CreateCompanyRequestHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = new Company(
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
                                request.CompanyTypEnumId,
                                request.CommissionRate,
                                request.AddTax,
                                request.GroupHeadId,
                                request.PriceContractId,
                                request.Kz,
                                request.StatusId,
                                request.Text
                                );
        company.DomainEvents.Add(EntityCreatedEvent.WithEntity(company));
        await _repository.AddAsync(company, cancellationToken);
        return company.Id;
    }
}