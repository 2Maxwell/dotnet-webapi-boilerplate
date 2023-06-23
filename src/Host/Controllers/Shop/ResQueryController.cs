using FSH.WebApi.Application.ShopMandant;
using FSH.WebApi.Application.ShopMandant.ResQuerys;

namespace FSH.WebApi.Host.Controllers.Shop;
public class ResQueryController : VersionedApiController
{
    [HttpPost("searchMandantResQuery")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search resQuery using available filters.", "")]
    public Task<CategoryRatesDto> SearchMandantResQueryAsync(SearchMandantResQueryRequest request)
    {
        return Mediator.Send(request);
    }
}
