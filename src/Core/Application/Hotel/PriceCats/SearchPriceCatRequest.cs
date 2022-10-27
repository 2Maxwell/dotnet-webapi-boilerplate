using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PriceCats;

public class SearchPriceCatsRequest : IRequest<List<PriceCatDto>>
{
    public int MandantId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int RateScopeEnumId { get; set; }
    public SearchPriceCatsRequest(int mandantId, DateTime start, DateTime end)
    {
        MandantId = mandantId;
        Start = start;
        End = end;
        // RateScopeEnumId = rateScopeEnumId;
    }
}

public class PriceCatsBySearchRequestSpec : Specification<PriceCat, PriceCatDto>
{
    public PriceCatsBySearchRequestSpec(SearchPriceCatsRequest request)
    {
        Query
        .Where(c => c.MandantId == request.MandantId)
        .Include(c => c.Category)
        .OrderBy(c => c.DatePrice)
        .ThenBy(c => c.CategoryId);
    }
}

public class SearchPriceCatsRequestHandler : IRequestHandler<SearchPriceCatsRequest, List<PriceCatDto>>
{
    private readonly IReadRepository<PriceCat> _repository;

    public SearchPriceCatsRequestHandler(IReadRepository<PriceCat> repository) => _repository = repository;

    public async Task<List<PriceCatDto>> Handle(SearchPriceCatsRequest request, CancellationToken cancellationToken)
    {
        var spec = new PriceCatsBySearchRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);
        return list;
    }
}