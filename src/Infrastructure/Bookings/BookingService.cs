using FSH.WebApi.Application.Accounting.Bookings;
using FSH.WebApi.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FSH.WebApi.Infrastructure.Bookings;
public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _db;

    public BookingService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<BookingDto>> GetBookingsForReport(int MandantId, DateTime StartDate, DateTime EndDate, int StartItemNumber, int EndItemNumber)
    {
        List<BookingDto> bookings = new();
        try
        {
            bookings = await _db.Booking.Where(x => x.MandantId == MandantId && x.BookingDate > StartDate && x.BookingDate < EndDate && x.ItemNumber > StartItemNumber && x.ItemNumber < EndItemNumber)
           .Select(x => new BookingDto
           {
               MandantId = x.MandantId,
               Amount = x.Amount,
               ItemNumber = x.ItemNumber,
               HotelDate = x.HotelDate,
               Price = x.Price,
               BookingLineNumberId = x.BookingLineNumberId,
               Source = x.Source

           }).ToListAsync();
        }
        catch (Exception)
        {

            throw;
        }


        return bookings;
    }
}
