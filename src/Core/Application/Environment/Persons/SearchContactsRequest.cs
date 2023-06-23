using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Persons;
public class SearchContactsRequest : PaginationFilter, IRequest<PaginationResponse<ContactDto>>
{
    public string? Name { get; set; }
    public int CompanyId { get; set; }
}

public class ContactsBySearchRequestWithNameCompanyIdSpec : EntitiesByPaginationFilterSpec<Person, ContactDto>
{
    public ContactsBySearchRequestWithNameCompanyIdSpec(SearchContactsRequest request)
        : base(request) =>
            Query
            .OrderBy(x => x.Name)
            .ThenBy(x => x.FirstName)
            .Where(x => x.MandantId == request.MandantId)
            .Where(x => x.CompanyId == request.CompanyId)
            .Where(x => x.Name!.StartsWith(request.Name!));
}

public class SearchContactsRequestHandler : IRequestHandler<SearchContactsRequest, PaginationResponse<ContactDto>>
{
    private readonly IReadRepository<Person> _repository;

    public SearchContactsRequestHandler(IReadRepository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResponse<ContactDto>> Handle(SearchContactsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ContactsBySearchRequestWithNameCompanyIdSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}
