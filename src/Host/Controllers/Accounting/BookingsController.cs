using DocumentFormat.OpenXml.Drawing.Charts;
using FSH.WebApi.Application.Accounting.Bookings;
using FSH.WebApi.Application.Reports;
using MediatR;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class BookingsController : VersionedApiController
{

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new Booking.", "")]
    public Task<int> CreateAsync(CreateBookingRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("createBookingBulk")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BulkBooking.", "")]
    public Task<bool> CreateBookingBulkAsync(CreateBookingBulkRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("searchByReservationId")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search Bookings by ReservationId.", "")]
    public Task<List<BookingDto>> SearchAsync(SearchBookingByReservationIdRequest request)
    {
        return Mediator.Send(request);
    }


}
