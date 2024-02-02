using FSH.WebApi.Application.Accounting.CashierRegisters;
using FSH.WebApi.Application.General.Appointments;

namespace FSH.WebApi.Host.Controllers.General;
public class AppointmentsController : VersionedApiController
{
    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new Appointment.", "")]
    public Task<int> CreateAsync(CreateAppointmentRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a Appointment.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateCashierRegisterRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get Appointment details.", "")]
    public Task<AppointmentDto> GetAsync(int id)
    {
        return Mediator.Send(new GetAppointmentRequest(id));
    }
}
