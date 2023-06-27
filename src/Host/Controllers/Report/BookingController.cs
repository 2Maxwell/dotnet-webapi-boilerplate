﻿using FSH.WebApi.Application.Accounting.Bookings;
using MediatR;

namespace FSH.WebApi.Host.Controllers.Report;

public class BookingController : VersionedApiController
{
    private readonly IMediator _mediator;

    public BookingController(MediatR.IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("bookingsReport")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBookingsReport([FromBody] BookingReportDto reportDto)
    {
        var req = new GetBookingReportRequest() { BookingReportDto = reportDto };

        var res = _mediator.Send(req);

        return Ok(res);
    }

    // Create a controller for GetReservationsReportRequest
    // I'm thinking about if it is better placed here or in the ReservationController
}
