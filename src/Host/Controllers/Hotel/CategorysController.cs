using FSH.WebApi.Application.Hotel.Categorys;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class CategorysController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search categorys using available filters.", "")]
    public Task<PaginationResponse<CategoryDto>> SearchAsync(SearchCategorysRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("getCategoryIsVirtualSelect/{selector:int}&{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search categorys for select virtual categorys filters.", "")]
    public Task<List<CategorySelectDto>> GetCategorySelectAsync(int selector, int mandantId)
    {
        return Mediator.Send(new GetCategorySelectIsVirtualRequest(selector, mandantId));
    }

    [HttpGet("searchCategoryPrices/{mandantId:int}&{start:DateTime}&{end:DateTime}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search categorys with priceCatRate.", "")]
    public Task<List<CategoryPriceDto>> SearchCategoryPricesAsync(int mandantId, DateTime start, DateTime end)
    {
        return Mediator.Send(new SearchCategoryPricesRequest(mandantId, start, end));
    }

    [HttpGet("getCategorySelectCatPax/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search categorys for select CatPax.", "")]
    public Task<List<CategorySelectCatPaxDto>> GetCategorySelectCatPaxAsync(int mandantId)
    {
        return Mediator.Send(new GetCategorySelectCatPaxRequest(mandantId));
    }

    [HttpGet("getCategoryByBeds/{mandantId:int}&{bedsRequired:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search categorys by bedsRequired.", "")]
    public Task<List<CategoryDto>> GetCategoryByBedsAsync(int mandantId, int bedsRequired)
    {
        return Mediator.Send(new GetCategoryByBedsRequest(mandantId, bedsRequired));
    }

    [HttpGet("getCategoryKzMultiSelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search categorys kz for multiSelect.", "")]
    public Task<List<CategoryKzMultiSelectDto>> GetCategoryKzMultiSelectAsync(int mandantId)
    {
        return Mediator.Send(new GetCategoryKzMultiSelectRequest(mandantId));
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get category details.", "")]
    public Task<CategoryDto> GetAsync(int id)
    {
        return Mediator.Send(new GetCategoryRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new category.", "")]
    public Task<int> CreateAsync(CreateCategoryRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a category.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateCategoryRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }
}
