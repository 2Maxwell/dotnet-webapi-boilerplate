using FSH.WebApi.Application.Accounting.Bookings;
using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Bookings;
public interface IBookingService
{
    Task<List<BookingDto>> GetBookingsForReport(int MandantId, DateTime StartDate, DateTime EndDate, int StartItemNumber, int EndItemNumber);
}
