using BoldReports.Writer;
using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Application.Hotel.Reservations;
using MediatR;

namespace FSH.WebApi.Host.Controllers.Report;
public class BoldReportWriterController : VersionedApiController
{
    private readonly IMediator _mediator;
    // IWebHostEnvironment used with sample to get the application data from wwwroot.
    private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

    // IWebHostEnvironment initialized with controller to get the data from application data folder.
    public BoldReportWriterController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment, IMediator mediator)
    {
        _hostingEnvironment = hostingEnvironment;
        _mediator = mediator;
    }

    // [HttpGet]
    [HttpPost("reservationsReportBoldReports")]
    [AllowAnonymous]
    public async Task<IActionResult> Export(GetReservationReportDtoRequest request)
    {
        // Here, we have loaded the sales-order-detail sample report from application the folder wwwroot\Resources.
        string writerFormat = request.ResponseType;
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Controllers/Report/BoldReportsTemplate", "Reservations.rdlc");
        var reservationReportDtos = await Mediator.Send(request);

        GetMandantDetailsRequest getMandantDetailRequest = new GetMandantDetailsRequest(request.MandantId);
        var mandantDetailDto = await Mediator.Send(getMandantDetailRequest);

        List<MandantDetailDto> mandants = new List<MandantDetailDto>();
        mandants.Add(mandantDetailDto);


        // FileStream inputStream = new FileStream(_hostingEnvironment.WebRootPath + @"\Resources\sales-order-detail.rdl", FileMode.Open, FileAccess.Read);
        FileStream inputStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        MemoryStream reportStream = new MemoryStream();
        inputStream.CopyTo(reportStream);
        reportStream.Position = 0;
        inputStream.Close();
        BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter();
        writer.ReportProcessingMode = ProcessingMode.Local;
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "Reservations", Value = reservationReportDtos });
        writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "MandantDetail", Value = mandants });

        string fileName = null;
        WriterFormat format;
        string type = null;

        if (writerFormat == "PDF")
        {
            fileName = "Reservations.pdf";
            type = "pdf";
            format = WriterFormat.PDF;
        }
        else if (writerFormat == "Word")
        {
            fileName = "sales-order-detail.doc";
            type = "doc";
            format = WriterFormat.Word;
        }
        else if (writerFormat == "CSV")
        {
            fileName = "sales-order-detail.csv";
            type = "csv";
            format = WriterFormat.CSV;
        }
        else
        {
            fileName = "Reservations.xls";
            type = "xls";
            format = WriterFormat.Excel;
        }

        writer.LoadReport(reportStream);
        MemoryStream memoryStream = new MemoryStream();
        writer.Save(memoryStream, format);

        // Download the generated export document to the client side.
        memoryStream.Position = 0;
        FileStreamResult fileStreamResult = new FileStreamResult(memoryStream, "application/" + type);
        fileStreamResult.FileDownloadName = fileName;
        return fileStreamResult;
    }

}
