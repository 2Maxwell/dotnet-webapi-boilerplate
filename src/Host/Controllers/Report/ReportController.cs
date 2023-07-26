using FSH.WebApi.Application.Accounting.Invoices;
using FSH.WebApi.Application.Environment.Persons;
using FSH.WebApi.Application.Hotel.Reservations;
using FSH.WebApi.Application.Reports;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Report;

public class ReportController : VersionedApiController
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("personAddressReport")]
    [AllowAnonymous]

    public async Task<IActionResult> GetPersonAddressReport(List<PersonAddressReportDto> data)
    {
        var req = new GenerateReportsRequest() { PersonAddressReportDtos = data };

        var res = await _mediator.Send(req);

        return Ok(res);
    }

    [HttpPost("reservationsReport")]
    [AllowAnonymous]
    [OpenApiOperation("Get a reservation Report.", "")]
    public async Task<IActionResult> GetReservationsReportAsync(GetReservationsReportRequest request)
    {
        // var req = new GetReservationsReportRequest() { ReservationListDto = reportDto };
        var res = await Mediator.Send(request);
        return Ok(res);
        //return Ok(_mediator.Send(request));
        // return Mediator.Send(request);
    }

    [HttpPost("invoiceReport")]
    [AllowAnonymous]
    [OpenApiOperation("Get a Invoice.", "")]
    public async Task<IActionResult> GetInvoiceReportAsync(GetInvoiceReportRequest request)
    {
        var res = await Mediator.Send(request);
        return Ok(res);
    }


    // Create a controller for GetReservationsReportRequest
    // I'm thinking about if it is better placed here or in the ReservationController


}
