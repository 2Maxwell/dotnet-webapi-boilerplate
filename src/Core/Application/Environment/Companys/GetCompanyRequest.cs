using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Companys;
public class GetCompanyRequest : IRequest<CompanyDto>
{
    public int Id { get; set; }
    public GetCompanyRequest(int id) => Id = id;
}

public class GetCompanyRequestHandler : IRequestHandler<GetCompanyRequest, CompanyDto>
{
    private readonly IRepository<Company> _repository;
    private readonly IStringLocalizer<GetCompanyRequestHandler> _localizer;

    public GetCompanyRequestHandler(IRepository<Company> repository, IStringLocalizer<GetCompanyRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<CompanyDto> Handle(GetCompanyRequest request, CancellationToken cancellationToken)
    {
        CompanyDto? companyDto = await _repository.GetBySpecAsync(
    (ISpecification<Company, CompanyDto>)new CompanyByIdSpec(request.Id), cancellationToken);

        if (companyDto == null) throw new NotFoundException(string.Format(_localizer["company.notfound"], request.Id));

        return companyDto;
    }
}

public class CompanyByIdSpec : Specification<Company, CompanyDto>, ISingleResultSpecification
{
    public CompanyByIdSpec(int id) => Query.Where(x => x.Id == id);
}