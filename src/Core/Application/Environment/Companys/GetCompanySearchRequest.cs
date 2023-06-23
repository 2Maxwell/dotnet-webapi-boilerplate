using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Companys;
public class GetCompanySearchRequest : IRequest<CompanySearchDto>
{
    public int Id { get; set; }
    public GetCompanySearchRequest(int id) => Id = id;
}

public class GetCompanySearchRequestHandler : IRequestHandler<GetCompanySearchRequest, CompanySearchDto>
{
    private readonly IRepository<Company> _repository;
    private readonly IStringLocalizer<GetCompanySearchRequestHandler> _localizer;

    public GetCompanySearchRequestHandler(IRepository<Company> repository, IStringLocalizer<GetCompanySearchRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<CompanySearchDto> Handle(GetCompanySearchRequest request, CancellationToken cancellationToken)
    {
        CompanySearchDto? companyDto = await _repository.GetBySpecAsync(
    (ISpecification<Company, CompanySearchDto>)new CompanySearchByIdSpec(request.Id), cancellationToken);

        if (companyDto == null) throw new NotFoundException(string.Format(_localizer["companySearch.notfound"], request.Id));

        return companyDto;
    }
}

public class CompanySearchByIdSpec : Specification<Company, CompanySearchDto>, ISingleResultSpecification
{
    public CompanySearchByIdSpec(int id) => Query.Where(x => x.Id == id);
}