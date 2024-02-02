using FSH.WebApi.Application.Accounting.Bookings;
using FSH.WebApi.Application.Accounting.Rates;
using FSH.WebApi.Domain.Accounting;

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

    [HttpPost("createBookingSplit")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new Split Booking.", "")]
    public Task<int> CreateBookingSplitAsync(CreateBookingSplitRequest request)
    {
        return Mediator.Send(request);
    }


    [HttpPost("createBookingsBulkInvoice")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new BookingsBulkInvoiceBooking.", "")]
    public Task<List<BookingDto>> CreateBookingBulkInvoiceAsync(CreateBookingsBulkInvoiceRequest request)
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

    [HttpPost("updateBookingBulk")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Update a BulkBooking.", "")]
    public Task<bool> UpdateBookingBulkAsync(UpdateBookingBulkRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a Booking.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateBookingRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }
}
