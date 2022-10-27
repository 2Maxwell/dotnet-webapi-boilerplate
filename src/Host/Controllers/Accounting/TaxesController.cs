using FSH.WebApi.Application.Accounting.Rates;
using FSH.WebApi.Application.Accounting.Taxes;
using FSH.WebApi.Application.Hotel.Categorys;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class TaxesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search Taxes using available filters.", "")]
    public Task<PaginationResponse<TaxDto>> SearchAsync(SearchTaxesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new Tax.", "")]
    public Task<int> CreateAsync(CreateTaxRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a Tax.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateTaxRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("getTaxSelectDto/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search taxes for select.", "")]
    public Task<List<TaxSelectDto>> GetTaxSelectDtoAsync(int mandantId)
    {
        return Mediator.Send(new GetTaxSelectRequest(mandantId));
    }

}
