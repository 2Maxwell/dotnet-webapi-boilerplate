using FSH.WebApi.Application.Accounting.Items;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class ItemPriceTaxController : VersionedApiController
{
    [HttpDelete("{id:int}")]
    // [MustHavePermission(FSHAction.Delete, FSHResource.Products)]
    [OpenApiOperation("Delete a itemPriceTax.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteItemPriceTaxRequest(id));
    }
}
