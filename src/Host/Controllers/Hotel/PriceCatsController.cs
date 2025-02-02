﻿using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Application.Hotel.PriceCats;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class PriceCatsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search PriceCats using available filters.", "")]
    public Task<List<PriceCatDto>> SearchAsync(SearchPriceCatsShopRequest request)
    {
        return Mediator.Send(request);
    }

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

    [HttpPost("createPriceCatDefault")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Create priceCatDefault.", "")]
    public async Task<bool> CreatePriceCatDefaultAsync(CreatePriceCatDefaultRequest request)
    {
        return await Mediator.Send(request);
    }

}
