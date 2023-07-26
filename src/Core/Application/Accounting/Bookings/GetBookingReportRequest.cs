using FSH.WebApi.Application.Environment.Persons;
using FSH.WebApi.Application.ReportsContract;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Infrastructure.Bookings;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FSH.WebApi.Application.Accounting.Bookings;
public class GetBookingReportRequest : IRequest<FileContentResult>
{
    public BookingReportDto BookingReportDto { get; set; }
}


public class GetBookingReportRequestHandler : IRequestHandler<GetBookingReportRequest, FileContentResult>
{
    private readonly IReportService<BookingDto> _reportService;
    private readonly IBookingService _bookingService;

    private const string ReportTemplatePath = "Bookings.frx";
    public GetBookingReportRequestHandler(IReportService<BookingDto> reportService, IBookingService bookingService)
    {
        _reportService = reportService;
        _bookingService = bookingService;
    }

    public async Task<FileContentResult> Handle(GetBookingReportRequest request, CancellationToken cancellationToken)
    {
        string dataRef = nameof(BookingDto) + "Ref";
        var dataDb = await _bookingService.GetBookingsForReport(request.BookingReportDto.MandantId, request.BookingReportDto.StartDate, request.BookingReportDto.EndDate, request.BookingReportDto.StartItemNumber, request.BookingReportDto.EndItemNumber);

        byte[] generatedReport = await _reportService.GenerateReport(ReportTemplatePath, dataDb, dataRef);

        return new FileContentResult(generatedReport, "application/pdf")
        {
            FileDownloadName = "GeneratedReport.pdf"
        };
    }

}
