using FSH.WebApi.Application.Accounting.Journals;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class JournalsController : VersionedApiController
{
    [HttpPost("getJournalByBookingId")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get journal details by BookingId.", "")]
    public Task<JournalDto> GetAsync(GetJournalByBookingIdRequest request)
    {
        return Mediator.Send(request);
    }
}
