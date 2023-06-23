using FSH.WebApi.Application.Environment.Countries;

namespace FSH.WebApi.Host.Controllers.Environment;
public class CountriesController : VersionedApiController
{
    [HttpGet("getCountrySelect")]
    //[AllowAnonymous]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search country for select.", "")]
    public Task<List<CountrySelectDto>> GetCountrySelectAsync()
    {
        return Mediator.Send(new GetCountrySelectRequest());
    }
}
