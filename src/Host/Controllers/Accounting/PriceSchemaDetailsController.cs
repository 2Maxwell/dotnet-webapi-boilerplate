using FSH.WebApi.Application.Accounting.PriceSchemaDetails;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class PriceSchemaDetailsController : VersionedApiController
{
    [HttpDelete("{id:int}")]
    // [MustHavePermission(FSHAction.Delete, FSHResource.Products)]
    [OpenApiOperation("Delete a priceSchemaDetail.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeletePriceSchemaDetailRequest(id));
    }
}
