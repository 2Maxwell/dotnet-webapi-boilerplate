using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Categorys;
public class GetCategoryByBedsRequest : IRequest<List<CategoryDto>>
{
    public GetCategoryByBedsRequest(int mandantId, int bedsRequired)
    {
        MandantId = mandantId;
        BedsRequired = bedsRequired;
    }

    public int MandantId { get; set; }
    public int BedsRequired { get; set; }
}

public class GetCategoryByBedsRequestHandler : IRequestHandler<GetCategoryByBedsRequest, List<CategoryDto>>
{
    private readonly IRepository<Category> _repository;
    private readonly IStringLocalizer<GetCategoryByBedsRequestHandler> _localizer;

    public GetCategoryByBedsRequestHandler(IRepository<Category> repository, IStringLocalizer<GetCategoryByBedsRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<CategoryDto>> Handle(GetCategoryByBedsRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Category, List<CategoryDto>>)new CategoryByBedsSpec(request.MandantId, request.BedsRequired), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["category.notfound"], request.MandantId));
}

public class CategoryByBedsSpec : Specification<Category, CategoryDto>, ISingleResultSpecification
{
    public CategoryByBedsSpec(int mandantId, int bedsRequired) =>
        Query.Where(x => x.MandantId == mandantId && (x.NumberOfBeds + x.NumberOfExtraBeds) >= bedsRequired);
}

