using FSH.WebApi.Application.General;
using FSH.WebApi.Application.Hotel.Reservations;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.General;
public class GeneralController : VersionedApiController
{
    [HttpPost("getNightAudit")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Get NightAudit for Mandant.", "")]
    public Task<GetNightAuditResponse> GetNightAuditAsync(GetNightAuditRequest request)
    {
        return Mediator.Send(request);
    }

}
