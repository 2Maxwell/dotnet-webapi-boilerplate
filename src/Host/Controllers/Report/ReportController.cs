using FSH.WebApi.Application.Environment.Persons;
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

        var res = _mediator.Send(req);

        return Ok(res);
    }
}
