using FSH.WebApi.Application.Hotel.PackageItems;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class PackageItemsController : VersionedApiController
{
    [HttpDelete("{id:int}")]
    // [MustHavePermission(FSHAction.Delete, FSHResource.Products)]
    [OpenApiOperation("Delete a packageItem.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeletePackageItemRequest(id));
    }
}
