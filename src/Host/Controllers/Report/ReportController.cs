using AspNetCore.Reporting;
using AspNetCore.Reporting.ReportExecutionService;
using BoldReports.Web.ReportViewer;
using BoldReports.Writer;
using DocumentFormat.OpenXml.Bibliography;
using FSH.WebApi.Application.Accounting.Invoices;
using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Application.Environment.Persons;
using FSH.WebApi.Application.Hotel.Reservations;
using FSH.WebApi.Application.Reports;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Host.Controllers.Report.ReportDataSets;
using Hangfire.Annotations;
using Mapster;
using MediatR;
using Microsoft.Extensions.Hosting.Internal;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text;

namespace FSH.WebApi.Host.Controllers.Report;

public class ReportController : VersionedApiController
{
    private readonly IMediator _mediator;
    private static List<Stream> m_streams;
    private static int m_currentPageIndex = 0;
    public Dictionary<string, string> parameterslocal { get; set; } = new Dictionary<string, string>();

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    #region "BoldReports"

    [HttpPost("rptreservations")]
    [AllowAnonymous]
    [OpenApiOperation("Get a rptreservation Report.", "")]
    public async Task<IActionResult> GetReservationsRptAsync(GetReservationReportDtoRequest request)
    {
        GetMandantDetailsRequest getMandantDetailRequest = new GetMandantDetailsRequest(request.MandantId);
        var mandantDetailDto = await Mediator.Send(getMandantDetailRequest);
        List<MandantDetailDto> mandants = new List<MandantDetailDto>
        {
            mandantDetailDto
        };

        var reservationReportDtos = await Mediator.Send(request);

        BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter();
        writer.ReportProcessingMode = BoldReports.Writer.ProcessingMode.Local;

        ReportSupplements reportSupplement = new ReportSupplements();
        reportSupplement.ReportTitle = "Reservation Report v1.0";
        reportSupplement.LogoPath = Path.Combine(Directory.GetCurrentDirectory(), "Controllers/Report/Images", "logoLunnax.png");
        reportSupplement.SearchString = "SearchQuery: Arrival: " + Convert.ToDateTime(request.Arrival).ToShortDateString() + ", Departure: " + Convert.ToDateTime(request.Departure).ToShortDateString() + ", ResKz: " + request.ResKz;
        List<ReportSupplements> reportSupplements = new List<ReportSupplements>
        {
            reportSupplement
        };

        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "Reservations", Value = reservationReportDtos });
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "MandantDetail", Value = mandants });
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "ReportSupplements", Value = reportSupplements });

        string fileName = "ReservationsReport";
        string writerFormat = request.ResponseType;
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Controllers/Report/BoldReportsTemplate", "Reservations.rdlc");

        FileContentResult fileContentResult = ProcessBoldReportWriter(writer, writerFormat, fileName, path);
        return Ok(fileContentResult);
    }

    [HttpPost("rptInvoiceCashier")]
    [AllowAnonymous]
    [OpenApiOperation("Get a rptInvoice by GetInvoiceReportRequest.", "")]
    public async Task<IActionResult> GetInvoiceRptAsync(GetInvoiceReportRequest request)
    {
        var invoiceReportDto = await Mediator.Send(request);

        InvoiceDto invoiceDto = invoiceReportDto.InvoiceDto!;
        List<InvoiceDto> invoiceDtos = new List<InvoiceDto>
        {
            invoiceDto
        };

        List<BookingLineSummary> bookingLineSummaries = new();
        List<BookingLine> bookingLines = new();
        foreach (var invoiceDetailDto in invoiceReportDto.InvoiceDetails)
        {
            BookingLine bookingLine = new BookingLine();
            bookingLine = invoiceDetailDto.Adapt<BookingLine>();  // Mapster Problem mit RefenrceId => BookingLineId
            bookingLine.DateBooking = invoiceDetailDto.HotelDate;
            bookingLine.BookingLineNumberId = invoiceDetailDto.BookingLineNumberId;
            bookingLines.Add(bookingLine);
        }

        foreach (var bookingLineGroup in bookingLines.Where(x => x.BookingLineNumberId != null).GroupBy(x => x.BookingLineNumberId))
        {
            BookingLineSummary bookingLineSummary = new BookingLineSummary();
            bookingLineSummary.SourceList = bookingLineGroup.ToList();
            bookingLineSummaries.Add(bookingLineSummary);
        }

        foreach (var bookingLineNonGroup in bookingLines.Where(x => x.BookingLineNumberId == null))
        {
            BookingLineSummary bookingLineSummary = new BookingLineSummary();
            List<BookingLine> bookingLineNonGroupList = new List<BookingLine>();
            bookingLineNonGroupList.Add(bookingLineNonGroup);
            bookingLineSummary.SourceList = bookingLineNonGroupList;
            bookingLineSummaries.Add(bookingLineSummary);
        }

        List<BookingLineSummaryReportResult> bookingLineSummaryReportResults = new List<BookingLineSummaryReportResult>();
        foreach (var bookingLineSummary in bookingLineSummaries)
        {
            BookingLineSummaryReportResult bookingLineSummaryReportResult = new BookingLineSummaryReportResult();
            bookingLineSummaryReportResult.Date = bookingLineSummary.Date;
            bookingLineSummaryReportResult.Amount = Convert.ToDecimal(bookingLineSummary.Amount);
            bookingLineSummaryReportResult.Price = Convert.ToDecimal(bookingLineSummary.Price);
            bookingLineSummaryReportResult.Total = Convert.ToDecimal(bookingLineSummary.Total);
            bookingLineSummaryReportResult.Description = bookingLineSummary.Description;
            bookingLineSummaryReportResult.ReferenceId = bookingLineSummary.ReferenceId;
            bookingLineSummaryReportResult.InvoicePosition = bookingLineSummary.InvoicePosition;
            bookingLineSummaryReportResult.TaxLine = bookingLineSummary.TaxLine;
            bookingLineSummaryReportResult.Debit = bookingLineSummary.Debit;
            bookingLineSummaryReportResults.Add(bookingLineSummaryReportResult);
        }

        InvoiceAddressDto invoiceAddressDto = invoiceReportDto.InvoiceAddressDto!;
        List<InvoiceAddressDto> invoiceAddressDtos = new List<InvoiceAddressDto>
        {
            invoiceAddressDto
        };

        List<InvoiceTaxDto> invoiceTaxDto = invoiceReportDto.InvoiceTaxDtos;

        List<InvoicePaymentDto> invoicePayments = invoiceReportDto.InvoicePaymentDtos;

        GetMandantDetailsRequest getMandantDetailRequest = new GetMandantDetailsRequest(request.MandantId);
        var mandantDetailDto = await Mediator.Send(getMandantDetailRequest);
        List<MandantDetailDto> mandants = new List<MandantDetailDto>
        {
            mandantDetailDto
        };

        ReportSupplements reportSupplement = new ReportSupplements();
        reportSupplement.ReportTitle = string.Empty;
        reportSupplement.LogoPath = Path.Combine(Directory.GetCurrentDirectory(), "Controllers/Report/Images", "logoLunnax.png");
        reportSupplement.SearchString = string.Empty;
        List<ReportSupplements> reportSupplements = new List<ReportSupplements>
        {
            reportSupplement
        };

        // TSE Daten laden

        BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter();
        writer.ReportProcessingMode = BoldReports.Writer.ProcessingMode.Local;
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "Invoice", Value = invoiceDtos });
        // writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "InvoiceDetails", Value = invoiceReportDto.InvoiceDetails });
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "InvoiceAddress", Value = invoiceAddressDtos });
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "InvoiceTax", Value = invoiceTaxDto });
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "InvoicePayment", Value = invoicePayments });
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "MandantDetail", Value = mandants });
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "ReportSupplements", Value = reportSupplements });
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "BookingLine", Value = bookingLineSummaryReportResults });

        string fileName = $"Invoice {invoiceDto.Id}-{invoiceDto.InvoiceIdMandant}";
        string writerFormat = request.ResponseType;
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Controllers/Report/BoldReportsTemplate", "InvoiceCashier.rdlc");

        FileContentResult fileContentResult = ProcessBoldReportWriter(writer, writerFormat, fileName, path);
        return Ok(fileContentResult);
    }

    private FileContentResult ProcessBoldReportWriter(ReportWriter writer, string writerFormat, string fileName, string path)
    {
        FileStream inputStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        MemoryStream reportStream = new MemoryStream();
        inputStream.CopyTo(reportStream);
        reportStream.Position = 0;
        inputStream.Close();

        WriterFormat format;
        string type = null;

        if (writerFormat == "PDF")
        {
            fileName += ".pdf";
            type = "pdf";
            format = WriterFormat.PDF;
        }
        else if (writerFormat == "Word")
        {
            fileName += ".doc";
            type = "doc";
            format = WriterFormat.Word;
        }
        else if (writerFormat == "CSV")
        {
            fileName += ".csv";
            type = "csv";
            format = WriterFormat.CSV;
        }
        else
        {
            fileName += ".xls";
            type = "xls";
            format = WriterFormat.Excel;
        }

        writer.LoadReport(reportStream);
        MemoryStream memoryStream = new MemoryStream();
        writer.Save(memoryStream, format);

        // Return the output as a byte array from the memory stream to the calling Method.
        memoryStream.Position = 0;
        return File(memoryStream.ToArray(), "application/" + type, fileName);

    }

    #endregion

    #region "FastReport Test"

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

    #endregion

    #region "BoldReports Test"

    [HttpPost("getreservationsReportDtoContent")]
    [AllowAnonymous]
    [OpenApiOperation("Get a reservation Report.", "")]
    public async Task<List<ReservationReportDto>> GetReservationReportDtoContentRequest(GetReservationReportDtoRequest request)
    {
        var reservationReportDtos = await Mediator.Send(request);
        return reservationReportDtos;
    }

    [HttpGet("getFullMandantReportDtoContentRequest")]
    [AllowAnonymous]
    [OpenApiOperation("Get FullMandant.", "")]
    public Task<MandantFullDto> GetFullMandantAsync(int mandantId)
    {
        return Mediator.Send(new GetFullMandantRequest(mandantId));
    }

    #endregion

    #region "RDLC Versuch"

    [HttpPost("reservationsReportRDLC")]
    [AllowAnonymous]
    [OpenApiOperation("Get a reservation Report.", "")]
    public async Task<IActionResult> PrintRptReservation(GetReservationReportDtoRequest request)
    {
        GetFullMandantRequest getFullMandantRequest = new GetFullMandantRequest(request.MandantId);
        var MandantFullDto = await Mediator.Send(getFullMandantRequest);
        dsMandantDetail dsmandantdetail = new dsMandantDetail();
        dsMandantDetail.dtMandantDetailsDataTable dtmd = new dsMandantDetail.dtMandantDetailsDataTable();
        MandantDetailDto mandantDetailDto = new MandantDetailDto();
        mandantDetailDto = MandantFullDto.MandantDetailDto;
        dtmd.Rows.Add(mandantDetailDto.Id, mandantDetailDto.MandantId, mandantDetailDto.Name1, mandantDetailDto.Name2, mandantDetailDto.Address1,
            mandantDetailDto.Address2, mandantDetailDto.Zip, mandantDetailDto.City, mandantDetailDto.CountryId, mandantDetailDto.StateRegionId,
            mandantDetailDto.Telephone, mandantDetailDto.Telefax, mandantDetailDto.Mobil, mandantDetailDto.Email, mandantDetailDto.EmailInvoice,
            mandantDetailDto.WebSite, mandantDetailDto.LanguageId, mandantDetailDto.BankName, mandantDetailDto.IBAN, mandantDetailDto.BIC,
            mandantDetailDto.TaxId, mandantDetailDto.UStId, mandantDetailDto.Company, mandantDetailDto.CompanyRegister);

        var reservationReportDtos = await Mediator.Send(request);
        dsReservations ds = new dsReservations();
        dsReservations.dtReservationsDataTable dt = new dsReservations.dtReservationsDataTable();

        foreach (var item in reservationReportDtos)
        {
            dt.Rows.Add(item.Id, item.MandantId, item.ResKz, item.Arrival, item.Departure, item.CategoryId, item.CategoryName, item.RoomAmount, item.RoomNumber, item.LogisTotal, item.BookingPolicyId, item.BookingPolicyName, item.CancellationPolicyId, item.CancellationPolicyName, item.IsGroupMaster, item.GroupMasterId, item.BookerName, item.GuestName, item.CompanyName);
        }

        string mimetype = "application/pdf";
        int extension = 1;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Controllers/Report/TemplateReports", "rpt_Reservations.rdlc");
        var pathLogo = Path.Combine(Directory.GetCurrentDirectory(), "Controllers/Report/Images", "logoLunnax.png");

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("prm1", "Reservations Report");
        parameters.Add("prmLogo", pathLogo);
        LocalReport localReport = new LocalReport(path);
        localReport.AddDataSource("DataSet1", dt);
        localReport.AddDataSource("DataSet2", dtmd);
        // localReport.EnableExternalImages = true;
        var reportFileByteString = localReport.Execute(GetRenderType(request.ResponseType!), extension, parameters, mimetype);

        // return File(reportFileByteString.MainStream, MediaTypeNames.Application.Octet, "ReservationReportRdlc.pdf");
        // return Ok(reportFileByteString.MainStream);
        FileContentResult fileContentResult = new FileContentResult(reportFileByteString.MainStream, "application/pdf");
        fileContentResult.FileDownloadName = getReportName("ReservationReportRDLC", request.ResponseType!);

        // return Ok(new FileContentResult(reportFileByteString.MainStream, "application/pdf"));
        // return Ok(new FileContentResult(reportFileByteString.MainStream, getReportName("ReservationReportRDLC", request.ResponseType!)));

        return Ok(fileContentResult);

    }

    private RenderType GetRenderType(string reportType)
    {
        var renderType = RenderType.Pdf;

        switch (reportType.ToUpper())
        {
            default:
            case "PDF":
                renderType = RenderType.Pdf;
                break;
            case "XLS":
                renderType = RenderType.Excel;
                break;
            case "WORD":
                renderType = RenderType.Word;
                break;
        }

        return renderType;
    }

    private string getReportName(string reportName, string reportType)
    {
        var outputFileName = reportName + ".pdf";

        switch (reportType.ToUpper())
        {
            default:
            case "PDF":
                outputFileName = reportName + ".pdf";
                break;
            case "XLS":
                outputFileName = reportName + ".xls";
                break;
            case "WORD":
                outputFileName = reportName + ".doc";
                break;
        }

        return outputFileName;
    }

    #endregion

    // Versuch directPrint zu implementieren - bisher nicht gelungen:
    // Problem: localReort.Execute liefert ein Byte-Array zurück, das nicht in ein Stream umgewandelt werden kann
    // oder nicht in den korekten Stream-Typ umgewandelt werden kann

    #region Versuch-directPrint-Implementierung
    public async void PrintReport(GetReservationReportDtoRequest request)
    {
        var reservationReportDtos = await Mediator.Send(request);
        dsReservations ds = new dsReservations();
        dsReservations.dtReservationsDataTable dt = new dsReservations.dtReservationsDataTable();

        foreach (var item in reservationReportDtos)
        {
            dt.Rows.Add(item.Id, item.MandantId, item.ResKz, item.Arrival, item.Departure, item.CategoryId, item.CategoryName, item.RoomAmount, item.RoomNumber, item.LogisTotal, item.BookingPolicyId, item.BookingPolicyName, item.CancellationPolicyId, item.CancellationPolicyName, item.IsGroupMaster, item.GroupMasterId, item.BookerName, item.GuestName, item.CompanyName);
        }
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Controllers/Report/TemplateReports", "rpt_Reservations.rdlc");
        // Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameterslocal.Add("prm1", "Reservations Report");
        LocalReport localReport = new LocalReport(path);
        localReport.AddDataSource("DataSet1", dt);

        PrintToPrinter(localReport);
    }


    public static void PrintToPrinter(LocalReport report)
    {
        Export(report);

    }

    public static void Export(LocalReport report, bool print = true)
    {
        string deviceInfo =
         @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>21cm</PageWidth>
                <PageHeight>29.7cm</PageHeight>
                <MarginTop>1.5cm</MarginTop>
                <MarginLeft>0.5cm</MarginLeft>
                <MarginRight>0.5cm</MarginRight>
                <MarginBottom>0.5cm</MarginBottom>
            </DeviceInfo>";
        Warning[] warnings;
        m_streams = new List<Stream>();

        // report.Render("Image", deviceInfo, CreateStream, out warnings);
        Dictionary<string, string> parametersExport = new Dictionary<string, string>();
        parametersExport.Add("prm1", "Reservations Report");

        var result = report.Execute(RenderType.Pdf, 1, parametersExport, "application/pdf");
        // var result = report.Execute(RenderType.Pdf, null, (name, extension, encoding, mimeType, willSeek) => stream, out _, out _, out _);
        // var result = report.Execute(RenderType.Pdf, 1, (name, extension, encoding, mimeType, _) => new MemoryStream());
        CreateStream("test", "pdf", Encoding.UTF8, "application/pdf", true);
        //m_streams.Add(new MemoryStream(result.MainStream));
        // Metafile pageImageTest = new Metafile(m_streams[0]);

        foreach (Stream stream in m_streams)
            stream.Position = 0;

        if (print)
        {
            Print();
        }
    }


    public static void Print()
    {
        if (m_streams == null || m_streams.Count == 0)
            throw new Exception("Error: no stream to print.");
        PrintDocument printDoc = new PrintDocument();
        if (!printDoc.PrinterSettings.IsValid)
        {
            throw new Exception("Error: cannot find the default printer.");
        }
        else
        {
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            m_currentPageIndex = 0;
            printDoc.Print();
        }
    }

    public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
    {
        Stream stream = new MemoryStream();
        m_streams.Add(stream);
        return stream;
    }

    public static void PrintPage(object sender, PrintPageEventArgs ev)
    {
        Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);

        // Adjust rectangular area with printer margins.
        Rectangle adjustedRect = new Rectangle(
            ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
            ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
            ev.PageBounds.Width,
            ev.PageBounds.Height);

        // Draw a white background for the report
        ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

        // Draw the report content
        ev.Graphics.DrawImage(pageImage, adjustedRect);

        // Prepare for the next page. Make sure we haven't hit the end.
        m_currentPageIndex++;
        ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
    }

    public static void DisposePrint()
    {
        if (m_streams != null)
        {
            foreach (Stream stream in m_streams)
                stream.Close();
            m_streams = null;
        }
    }
    #endregion

}
