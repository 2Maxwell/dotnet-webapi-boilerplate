using FSH.WebApi.Application.Environment.StateRegions;

namespace FSH.WebApi.Host.Controllers.Environment;
public class StateRegionsController : VersionNeutralApiController
{
    [HttpGet("getStateRegionsSelect/{countryId:int}")]
    [AllowAnonymous]
    //[MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search stateRegions for select.", "")]
    public Task<List<StateRegionSelectDto>> GetStateRegionsSelectAsync(int countryId)
    {
        return Mediator.Send(new GetStateRegionsSelectRequest(countryId));
    }
}
