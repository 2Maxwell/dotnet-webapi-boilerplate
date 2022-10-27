using FSH.WebApi.Application.Hotel.PriceCats;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class PriceCatsController : VersionedApiController
{
    [HttpPost]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update priceCats.", "")]
    public async Task<int> UpdateAsync(UpdatePriceCatBulkRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("updatePriceCatList")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update priceCatsList.", "")]
    public async Task<int> UpdateListAsync(UpdatePriceCatsListRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpPut("updatePriceCatSinglePrice{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update priceCatSinglePrice.", "")]
    public async Task<ActionResult<int>> UpdateSingleAsync(UpdatePriceCatSinglePriceRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }
}
