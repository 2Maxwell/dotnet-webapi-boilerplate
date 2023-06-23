using FSH.WebApi.Application.Environment.Persons;
using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Companys;
public class SearchCompanysRequest : PaginationFilter, IRequest<PaginationResponse<CompanySearchDto>>
{
    public string? Name1
    {
        get
        {
            string value = string.Empty;
            if (SearchString != null)
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

    public string? City
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

public class CompanysBySearchRequestWithNameFirstNameZipSpec : EntitiesByPaginationFilterSpec<Company, CompanySearchDto>
{
    public CompanysBySearchRequestWithNameFirstNameZipSpec(SearchCompanysRequest request)
        : base(request) =>
            Query
            .OrderBy(x => x.Name1)
            .ThenBy(x => x.Zip)
            .Where(x => x.MandantId == request.MandantId)
            .Where(x => x.Name1!.StartsWith(request.Name1!))
            .Where(x => x.Zip!.StartsWith(request.Zip!), request.Zip != string.Empty)
            .Where(x => x.City!.StartsWith(request.City!), request.City != string.Empty);
}

public class SearchCompanysRequestHandler : IRequestHandler<SearchCompanysRequest, PaginationResponse<CompanySearchDto>>
{
    private readonly IReadRepository<Company> _repository;

    public SearchCompanysRequestHandler(IReadRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResponse<CompanySearchDto>> Handle(SearchCompanysRequest request, CancellationToken cancellationToken)
    {
        var spec = new CompanysBySearchRequestWithNameFirstNameZipSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}