using FSH.WebApi.Application.Accounting.TaxItems;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class TaxItemsController : VersionedApiController
{
    [HttpDelete("{id:int}")]
    // [MustHavePermission(FSHAction.Delete, FSHResource.Products)]
    [OpenApiOperation("Delete a taxItem.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteTaxItemRequest(id));
    }
}
