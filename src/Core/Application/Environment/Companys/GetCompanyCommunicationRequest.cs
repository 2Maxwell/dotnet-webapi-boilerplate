using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Companys;
public class GetCompanyCommunicationRequest : IRequest<CompanyCommunicationDto>
{
    public int Id { get; set; }
    public GetCompanyCommunicationRequest(int id) => Id = id;
}

public class GetCompanyCommunicationRequestHandler : IRequestHandler<GetCompanyCommunicationRequest, CompanyCommunicationDto>
{
    private readonly IRepository<Company> _repository;
    private readonly IStringLocalizer<GetCompanyCommunicationRequestHandler> _localizer;

    public GetCompanyCommunicationRequestHandler(IRepository<Company> repository, IStringLocalizer<GetCompanyCommunicationRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<CompanyCommunicationDto> Handle(GetCompanyCommunicationRequest request, CancellationToken cancellationToken)
    {
        CompanyCommunicationDto? companyDto = await _repository.GetBySpecAsync(
    (ISpecification<Company, CompanyCommunicationDto>)new CompanyCommunicationByIdSpec(request.Id), cancellationToken);

        if (companyDto == null) throw new NotFoundException(string.Format(_localizer["company.notfound"], request.Id));

        return companyDto;
    }
}

public class CompanyCommunicationByIdSpec : Specification<Company, CompanyCommunicationDto>, ISingleResultSpecification
{
    public CompanyCommunicationByIdSpec(int id) => Query.Where(x => x.Id == id);
}