using FSH.WebApi.Domain.Environment;
namespace FSH.WebApi.Application.Environment.Languages;

public class SearchLanguagesRequest : PaginationFilter, IRequest<PaginationResponse<LanguageDto>>
{
}

public class LanguagesBySearchRequestSpec : EntitiesByPaginationFilterSpec<Language, LanguageDto>
{
    public LanguagesBySearchRequestSpec(SearchLanguagesRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchLanguagesRequestHandler : IRequestHandler<SearchLanguagesRequest, PaginationResponse<LanguageDto>>
{
    private readonly IReadRepository<Language> _repository;

    public SearchLanguagesRequestHandler(IReadRepository<Language> repository) => _repository = repository;

    public async Task<PaginationResponse<LanguageDto>> Handle(SearchLanguagesRequest request, CancellationToken cancellationToken)
    {
        var spec = new LanguagesBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<LanguageDto>(list, count, request.PageNumber, request.PageSize);
    }
}