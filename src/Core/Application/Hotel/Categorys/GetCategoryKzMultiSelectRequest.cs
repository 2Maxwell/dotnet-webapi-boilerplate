using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class GetCategoryKzMultiSelectRequest : IRequest<List<CategoryKzMultiSelectDto>>
{
    public int MandantId { get; set; }
    public GetCategoryKzMultiSelectRequest(int mandantId) => MandantId = mandantId;
}

public class CategoryKzByMandantIdSpec : Specification<Category, CategoryKzMultiSelectDto>
{
public CategoryKzByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId)
             .OrderBy(c => c.OrderNumber);
    }
}

public class GetCategoryKzMultiSelectRequestHandler : IRequestHandler<GetCategoryKzMultiSelectRequest, List<CategoryKzMultiSelectDto>>
{
    private readonly IRepository<Category> _repository;
    private readonly IStringLocalizer<GetCategoryKzMultiSelectRequestHandler> _localizer;

    public GetCategoryKzMultiSelectRequestHandler(IRepository<Category> repository, IStringLocalizer<GetCategoryKzMultiSelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);
    public async Task<List<CategoryKzMultiSelectDto>> Handle(GetCategoryKzMultiSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Category, CategoryKzMultiSelectDto>)new CategoryKzByMandantIdSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["CategorysKz.notfound"], request.MandantId));
}
