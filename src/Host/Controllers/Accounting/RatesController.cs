using FSH.WebApi.Application.Accounting.Rates;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class RatesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search Rates using available filters.", "")]
    public Task<PaginationResponse<RateDto>> SearchAsync(SearchRatesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new Rate.", "")]
    public Task<int> CreateAsync(CreateRateRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a Rate.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateRateRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get Rate details.", "")]
    public Task<RateDto> GetAsync(int id)
    {
        return Mediator.Send(new GetRateRequest(id));
    }

    [HttpPost("getRateShopMandantRecalculateRequest")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("get recalculated RateShopMandantDto after Packages change ", "")]
    public Task<RateShopMandantDto> GetRateShopMandantRecalculateRequestAsync(GetRateShopMandantRecalculateRequest request)
    {
        return Mediator.Send(request);
    }

}
