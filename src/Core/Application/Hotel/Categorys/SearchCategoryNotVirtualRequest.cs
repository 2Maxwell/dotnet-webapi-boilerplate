using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class SearchCategoryNotVirtualRequest : BaseFilter, IRequest<List<CategorySelectDto>>
{
    public int MandantId { get; set; }
}

public class CategoryNotVirtualRequestSpec : EntitiesByBaseFilterSpec<Category, CategorySelectDto>
{
    public CategoryNotVirtualRequestSpec(SearchCategoryNotVirtualRequest request)
: base(request) =>
        Query.Where(c => !c.CategoryIsVirtual && c.MandantId == request.MandantId);
}

public class SearchCategoryNotVirtualRequestHandler : IRequestHandler<SearchCategoryNotVirtualRequest, List<CategorySelectDto>>
{
    private readonly IReadRepository<Category> _repository;
    public SearchCategoryNotVirtualRequestHandler(IReadRepository<Category> repository) => _repository = repository;

    public async Task<List<CategorySelectDto>> Handle(SearchCategoryNotVirtualRequest request, CancellationToken cancellationToken)
    {
        var spec = new CategoryNotVirtualRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);
        return list;

        // throw new NotImplementedException();
    }
}