using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PriceCats;
public class SearchPriceCatsShopRequest : IRequest<List<PriceCatDto>>
{
    public int MandantId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public int Pax { get; set; }
    public int CategoryId { get; set; }
}

public class PriceCatShopByShopRequestSpec : Specification<PriceCat, PriceCatDto>
{
    public PriceCatShopByShopRequestSpec(SearchPriceCatsShopRequest request)
    {
        Query
            .Where(x => x.MandantId == request.MandantId &&
                x.Pax >= request.Pax &&
                x.CategoryId == request.CategoryId &&
                (x.DatePrice >= request.Start && x.DatePrice <= request.End))
            .OrderBy(x => x.CategoryId)
            .ThenBy(x => x.Pax)
            .ThenBy(x => x.DatePrice);
    }
}

public class SearchPriceCatsShopRequestHandler : IRequestHandler<SearchPriceCatsShopRequest, List<PriceCatDto>>
{
    private readonly IReadRepository<PriceCat> _repository;

    public SearchPriceCatsShopRequestHandler(IReadRepository<PriceCat> repository) => _repository = repository;

    public async Task<List<PriceCatDto>> Handle(SearchPriceCatsShopRequest request, CancellationToken cancellationToken)
    {
        var spec = new PriceCatShopByShopRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);
        return list;
    }
}
