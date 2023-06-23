using FSH.WebApi.Application.Accounting.Mandants;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class MandantsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Mandants)]
    [OpenApiOperation("Search mandants using available filters.", "")]
    public Task<PaginationResponse<MandantDto>> SearchAsync(SearchMandantRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Mandants)]
    [OpenApiOperation("Get mandant details.", "")]
    public Task<MandantDto> GetAsync(int id)
    {
        return Mediator.Send(new GetMandantRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Mandants)]
    [OpenApiOperation("Create a new mandant.", "")]
    public Task<int> CreateAsync(CreateMandantRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Mandants)]
    [OpenApiOperation("Update a mandant.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateMandantRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("getMandantNumber")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get MandantNumbers.", "NumberTyp: GroupMaster, Phantom, Invoice, Journal, Reservation")]
    public Task<int> GetMandantNumberAsync(GetMandantNumberRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("getMandantSetting")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get MandantSetting.", "")]
    public Task<MandantSettingDto> GetMandantSettingAsync(int mandantId)
    {
        return Mediator.Send(new GetMandantSettingRequest(mandantId));
    }
}
