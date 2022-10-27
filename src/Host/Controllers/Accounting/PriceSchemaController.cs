using FSH.WebApi.Application.Accounting.PriceSchemas;
using FSH.WebApi.Application.Hotel.Categorys;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class PriceSchemaController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search priceSchemas using available filters.", "")]
    public Task<PaginationResponse<PriceSchemaDto>> SearchAsync(SearchPriceSchemasRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get priceSchema details.", "")]
    public Task<PriceSchemaDto> GetAsync(int id)
    {
        return Mediator.Send(new GetPriceSchemaRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new priceSchemae.", "")]
    public Task<int> CreateAsync(CreatePriceSchemaRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a priceSchema.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdatePriceSchemaRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("getPriceSchemaSelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search priceSchema for select.", "")]
    public Task<List<PriceSchemaDto>> GetPriceSchemaSelectAsync(int mandantId)
    {
        return Mediator.Send(new GetPriceSchemaSelectRequest(mandantId));
    }
}
