using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.VCats;
public class SearchVCatRequest : IRequest<List<VCatDto>>
{
    public int MandantId { get; set; }
    public DateTime StartDate { get; set; }

    internal int days = 30;
}

public class VCatBySearchRequestSpec : Specification<VCat, VCatDto>
{
    public VCatBySearchRequestSpec(SearchVCatRequest request) =>
               Query
               .Where(c => c.MandantId == request.MandantId)
               .Where(c => c.Date >= request.StartDate && c.Date <= request.StartDate.AddDays(request.days))
               .OrderBy(c => c.Date)
               .ThenBy(c => c.CategoryId);
}

public class SearchVCatRequestHandler : IRequestHandler<SearchVCatRequest, List<VCatDto>>
{
    private readonly IReadRepository<VCat> _repository;

    public SearchVCatRequestHandler(IReadRepository<VCat> repository) => _repository = repository;
    public async Task<List<VCatDto>> Handle(SearchVCatRequest request, CancellationToken cancellationToken)
    {
        var spec = new VCatBySearchRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);

        return list;
    }
}
