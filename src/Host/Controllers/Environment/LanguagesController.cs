using FSH.WebApi.Application.Environment.Languages;

namespace FSH.WebApi.Host.Controllers.Environment;
public class LanguagesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search Languages using available Filters.", "")]
    public Task<PaginationResponse<LanguageDto>> SearchAsync(SearchLanguagesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get language details.", "")]
    public Task<LanguageDto> GetAsync(int id)
    {
        return Mediator.Send(new GetLanguageRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new language.", "")]
    public Task<int> CreateAsync(CreateLanguageRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a language.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateLanguageRequest request, int id)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return Ok(await Mediator.Send(request));
    }

    // nicht implementiert
    // [HttpDelete("{id:int}")]
    // [MustHavePermission(FSHPermissions.Brands.Delete)]
    // [OpenApiOperation("Delete a language.", "")]
    // public Task<int> DeleteAsync(int id)
    // {
    //     return Mediator.Send(new DeleteLanguageRequest(id));
    // }
}
