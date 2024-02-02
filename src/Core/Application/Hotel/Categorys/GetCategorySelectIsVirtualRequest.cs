using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class GetCategorySelectIsVirtualRequest : IRequest<List<CategorySelectDto>>
{
    public int Selector { get; set; } // Selector -1 = IsVirtual False, 0 = IsVirtual egal, 1 = IsVirtual = True
    public int MandantId { get; set; }
    public GetCategorySelectIsVirtualRequest(int selector, int mandantId) =>
        (Selector, MandantId) = (selector, mandantId);
}

public class CategoryByIsVirtualSpec : Specification<Category, CategorySelectDto>, ISingleResultSpecification
{
    // public CategoryByIsVirtualSpec(int selector) =>
    //    Query.Where(p => !p.CategoryIsVirtual);
    public CategoryByIsVirtualSpec(int selector, int mandantId)
    {
        if (selector < 0)
        {
            Query.Where(p => !p.CategoryIsVirtual && p.MandantId == mandantId && p.VkatRelevant);
        }
        else if (selector == 0)
        {
            Query.Where(p => p.MandantId == mandantId && p.VkatRelevant);
        }
        else
        {
            Query.Where(p => p.CategoryIsVirtual && p.MandantId == mandantId && p.VkatRelevant);
        }
    }
}

public class GetCategorySelectIsVirtualRequestHandler : IRequestHandler<GetCategorySelectIsVirtualRequest, List<CategorySelectDto>>
{
    private readonly IRepository<Category> _repository;
    private readonly IStringLocalizer<GetCategorySelectIsVirtualRequestHandler> _localizer;

    public GetCategorySelectIsVirtualRequestHandler(IRepository<Category> repository, IStringLocalizer<GetCategorySelectIsVirtualRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<CategorySelectDto>> Handle(GetCategorySelectIsVirtualRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Category, CategorySelectDto>)new CategoryByIsVirtualSpec(request.Selector, request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["IsVirtualCategorys.notfound"], request.Selector));
}