using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Bookings;
public class BookingReportDto
{
    public int MandantId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int StartItemNumber { get; set; }
    public int EndItemNumber { get; set; }

}
