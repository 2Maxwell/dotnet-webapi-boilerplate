using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Application.Hotel.Packages;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class PackagesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search packages using available filters.", "")]
    public Task<PaginationResponse<PackageDto>> SearchAsync(SearchPackagesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get package details.", "")]
    public Task<PackageDto> GetAsync(int id)
    {
        return Mediator.Send(new GetPackageRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new package.", "")]
    public Task<int> CreateAsync(CreatePackageRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a package.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdatePackageRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("getPackageKzMultiSelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search packages kz for multiSelect.", "")]
    public Task<List<PackageKzMultiSelectDto>> GetPackageKzMultiSelectAsync(int mandantId)
    {
        return Mediator.Send(new GetPackageKzMultiSelectRequest(mandantId));
    }

}
