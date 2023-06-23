using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Salutations;
public class SearchSalutationsRequest : PaginationFilter, IRequest<PaginationResponse<SalutationDto>>
{
}

public class SalutationsSearchRequestSpec : EntitiesByPaginationFilterSpec<Salutation, SalutationDto>
{
    public SalutationsSearchRequestSpec(SearchSalutationsRequest request)
        : base(request) =>
        Query
        .OrderBy(p => p.LanguageId, !request.HasOrderBy())
        .ThenBy(p => p.OrderNumber);
}

public class SearchSalutationsRequestHandler : IRequestHandler<SearchSalutationsRequest, PaginationResponse<SalutationDto>>
{
    private readonly IReadRepository<Salutation> _repository;

    public SearchSalutationsRequestHandler(IReadRepository<Salutation> repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResponse<SalutationDto>> Handle(SearchSalutationsRequest request, CancellationToken cancellationToken)
    {
        var spec = new SalutationsSearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);
        return new PaginationResponse<SalutationDto>(list, count, request.PageNumber, request.PageSize);
    }
}
