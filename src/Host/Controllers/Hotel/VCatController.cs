using FSH.WebApi.Application.Hotel.Rooms;
using FSH.WebApi.Application.Hotel.VCats;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class VCatController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search VCat using available filters.", "")]
    public Task<List<VCatDto>> SearchAsync(SearchVCatRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("calculateVCat/{mandantId:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Calculate VCategory new.", "")]
    public Task<int> CalculateVCatAsync(int mandantId)
    {
        return Mediator.Send(new CalculateVCatRequest(mandantId));
    }

}
