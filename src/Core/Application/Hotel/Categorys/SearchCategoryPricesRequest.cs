using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class SearchCategoryPricesRequest : IRequest<List<CategoryPriceDto>>
{
    public int MandantId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int RateScopeEnumId { get; set; }
    public SearchCategoryPricesRequest(int mandantId, DateTime start, DateTime end)
    {
        MandantId = mandantId;
        Start = start;
        End = end;
    }
}

public class CategoryPricesBySearchRequestSpec : Specification<Category, CategoryPriceDto>
{
    public CategoryPricesBySearchRequestSpec(SearchCategoryPricesRequest request)
    {
        Query
        .Where(c => c.MandantId == request.MandantId)
        .OrderBy(c => c.OrderNumber);
    }
}

public class PriceCatsByCategorySpec : Specification<PriceCat, PriceCatDto>
{
    public PriceCatsByCategorySpec(int categoryId, DateTime start, DateTime end)
    {
        Query
            .Where(x => x.CategoryId == categoryId && x.DatePrice >= start && x.DatePrice <= end.AddDays(1))
            .OrderBy(x => x.DatePrice)
            .ThenBy(x => x.CategoryId);
    }
}

public class SearchCategoryPricesRequestHandler : IRequestHandler<SearchCategoryPricesRequest, List<CategoryPriceDto>>
{
    private readonly IReadRepository<Category> _repository;
    private readonly IReadRepository<PriceCat> _priceCatRateRepository;


    public SearchCategoryPricesRequestHandler(IReadRepository<Category> repository, IReadRepository<PriceCat> priceCatRateRepository) =>
        (_repository, _priceCatRateRepository) = (repository, priceCatRateRepository);

    public async Task<List<CategoryPriceDto>> Handle(SearchCategoryPricesRequest request, CancellationToken cancellationToken)
    {
        var spec = new CategoryPricesBySearchRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);

        foreach (CategoryPriceDto item in list)
        {
            //List<PriceCatDto> listPriceCat = new List<PriceCatDto>();
            var specDetail = new PriceCatsByCategorySpec(item.Id, request.Start, request.End);
            var listPriceCat = await _priceCatRateRepository.ListAsync(specDetail, cancellationToken); // (ISpecification<PriceCat, PriceCatDto>)new PriceCatsByCategorySpec(item.Id, request.Start, request.End), cancellationToken);

            item.PriceCats = listPriceCat;
        }

        return list;
    }
}