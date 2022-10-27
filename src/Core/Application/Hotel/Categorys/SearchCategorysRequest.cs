using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class SearchCategorysRequest : PaginationFilter, IRequest<PaginationResponse<CategoryDto>>
{
    // public int MandantId { get; set; }
}

public class CategorysBySearchRequestSpec : EntitiesByPaginationFilterSpec<Category, CategoryDto>
{
    public CategorysBySearchRequestSpec(SearchCategorysRequest request)
        : base(request) =>
        Query.Where(c => c.MandantId == request.MandantId)
             .OrderBy(c => c.OrderNumber, !request.HasOrderBy());
}

public class SearchCategorysRequestHandler : IRequestHandler<SearchCategorysRequest, PaginationResponse<CategoryDto>>
{
    private readonly IReadRepository<Category> _repository;
    private readonly IDapperRepository _roomRepository;
    public SearchCategorysRequestHandler(IReadRepository<Category> repository, IDapperRepository roomRepository)
    {
        _repository = repository;
        _roomRepository = roomRepository;
    }

    public async Task<PaginationResponse<CategoryDto>> Handle(SearchCategorysRequest request, CancellationToken cancellationToken)
    {
        var spec = new CategorysBySearchRequestSpec(request);
        var list = await _repository.ListAsync(spec, cancellationToken);
        foreach (CategoryDto dto in list)
        {
            dto.VirtualCategoryQuantity = await _roomRepository.QueryExecuteScalarAsync<int>(
                $"SELECT COUNT(Id) FROM Lnx.Room WHERE CategoryId = {dto.Id}", cancellationToken);
        }

        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<CategoryDto>(list, count, request.PageNumber, request.PageSize);
    }
}