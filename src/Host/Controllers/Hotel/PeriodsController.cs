using FSH.WebApi.Application.Hotel.Periods;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class PeriodsController : VersionedApiController
{
    [HttpDelete("{id:int}")]
    // [MustHavePermission(FSHAction.Delete, FSHResource.Products)]
    [OpenApiOperation("Delete a period.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeletePeriodRequest(id));
    }
}
