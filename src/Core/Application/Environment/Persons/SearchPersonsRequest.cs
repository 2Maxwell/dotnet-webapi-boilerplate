using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Persons;
public class SearchPersonsRequest : PaginationFilter, IRequest<PaginationResponse<PersonDto>>
{
    public string? Name
    {
        get
        {
            string value = string.Empty;
            if (SearchString != null) // && SearchString.Contains(','))
            {
                if (SearchString.Contains(','))
                {
                    string[] arr = SearchString.Split(',');
                    return arr[0].Trim();
                }

                return SearchString;
            }

            return value;
        }
    }

    public string? FirstName
    {
        get
        {
            string value = string.Empty;
            if (SearchString != null)
            {
                if (SearchString.Contains(','))
                {
                    string[] arr = SearchString.Split(',');
                    if (arr.Length > 1)
                    {
                        if (arr[1] != null)
                        {
                            return arr[1].Trim();
                        }
                    }
                }
            }

            return value;
        }
    }

    public string? Zip
    {
        get
        {
            string value = string.Empty;
            if (SearchString != null)
            {
                if (SearchString.Contains(','))
                {
                    string[] arr = SearchString.Split(',');
                    if (arr.Length > 2)
                    {
                        if (arr[2] != null)
                        {
                            return arr[2].Trim();
                        }
                    }
                }
            }

            return value;
        }
    }

    public string? SearchString { get; set; }
}

public class PersonsBySearchRequestWithNameFirstNameZipSpec : EntitiesByPaginationFilterSpec<Person, PersonDto>
{
    public PersonsBySearchRequestWithNameFirstNameZipSpec(SearchPersonsRequest request)
        : base(request) =>
            Query
            .OrderBy(x => x.Name)
            .ThenBy(x => x.FirstName)
            .Where(x => x.MandantId == request.MandantId)
            .Where(x => x.Name!.StartsWith(request.Name!))
            .Where(x => x.FirstName!.StartsWith(request.FirstName!), request.FirstName != string.Empty)
            .Where(x => x.Zip!.StartsWith(request.Zip!), request.Zip != string.Empty);
}

public class SearchPersonsRequestHandler : IRequestHandler<SearchPersonsRequest, PaginationResponse<PersonDto>>
{
    private readonly IReadRepository<Person> _repository;

    public SearchPersonsRequestHandler(IReadRepository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResponse<PersonDto>> Handle(SearchPersonsRequest request, CancellationToken cancellationToken)
    {
        var spec = new PersonsBySearchRequestWithNameFirstNameZipSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}