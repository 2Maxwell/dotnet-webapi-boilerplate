using FSH.WebApi.Application.Accounting.CashierRegisters;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class CashierRegisterController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search CashierRegister using available filters.", "")]
    public Task<PaginationResponse<CashierRegisterDto>> SearchAsync(SearchCashierRegisterRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new CashierRegister.", "")]
    public Task<int> CreateAsync(CreateCashierRegisterRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a CashierRegister.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateCashierRegisterRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get CashierRegister details.", "")]
    public Task<CashierRegisterDto> GetAsync(int id)
    {
        return Mediator.Send(new GetCashierRegisterRequest(id));
    }

    [HttpGet("getCashierRegisterDtos/{mandantId:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get CashierRegisterDtosList.", "")]
    public Task<List<CashierRegisterDto>> GetCashierRegisterDtosRequest(int mandantId)
    {
        return Mediator.Send(new GetCashierRegisterDtosRequest(mandantId));
    }

}