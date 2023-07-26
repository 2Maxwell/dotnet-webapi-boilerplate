using FastReport;
using FSH.WebApi.Application.Hotel.Reservations;
using FSH.WebApi.Application.ReportsContract;

namespace FSH.WebApi.Infrastructure.Reports;
public class ReportService<T> : IReportService<T> where T : class
{
    public async Task<byte[]> GenerateReport(string templatePath, List<T> data, string dataRef)
    {
        using (var report = new Report())
        {
            // Load the report template
            report.Load($"Controllers/Report/TemplateReports/{templatePath}");

            List<ReservationListDto> reservationsListDto = data as List<ReservationListDto>;

            // Assign data to the report
            report.RegisterData(data, dataRef);

            // Generate the report
            report.Prepare();

            // Export the report to a PDF format
            using (var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport())
            {
                var exportedStream = new System.IO.MemoryStream();
                pdfExport.Export(report, exportedStream);

                return exportedStream.ToArray();
            }
        }
    }


    public async Task<byte[]> GenerateReportInvoice(string templatePath, List<T> invoiceReportDto, string dataRef)
    {
        using (var report = new Report())
        {

            //report.Dictionary.RegisterBusinessObject(
            //   (System.Collections.IEnumerable)invoiceReportDto, // a (empty) list of objects
            //   "invoiceReportDto",          // name of dataset
            //   2,                   // depth of navigation into properties
            //   true                 // enable data source
            //   );
            //report.Save(@"invoiceReportAutoBuild_2.frx");

            // Load the report template
            // report.Load($"Controllers/Report/TemplateReports/{templatePath}");
            report.Load($@"{Directory.GetCurrentDirectory()}\Controllers\Report\TemplateReports\{templatePath}");

            // InvoiceReportDto invoiceReportDto = data[0] as InvoiceReportDto;
            // List<T> data = new List<T>();
            // Assign data to the report
            report.RegisterData(invoiceReportDto, dataRef);

            // Generate the report
            report.Prepare();

            // Export the report to a PDF format
            using (var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport())
            {
                var exportedStream = new System.IO.MemoryStream();
                pdfExport.Export(report, exportedStream);

                // return await Task.FromResult(exportedStream.ToArray());
                return exportedStream.ToArray();

            }
        }
    }

    //public async Task<byte[]> GenerateReportInvoice(string templatePath, List<T> data, string dataRef)
    //{
    //    // var report = new Report();
    //    //report.Dictionary.RegisterBusinessObject(
    //    //              new List<Invoice>(), // a (empty) list of objects
    //    //              "Invoices",          // name of dataset
    //    //              2,                   // depth of navigation into properties
    //    //              true                 // enable data source
    //    //       );
    //    //report.Save(@"invoiceReportAuto.frx");
    //    using (var report = new Report())
    //    {
    //        report.Dictionary.RegisterBusinessObject(
    //          data, // a (empty) list of objects
    //          "InvoiceReportDto",          // name of dataset
    //          2,                   // depth of navigation into properties
    //          true                 // enable data source
    //   );
    //        report.Save(@"invoiceReportAuto.frx");

    //        // Load the report template
    //        report.Load($"Controllers/Report/TemplateReports/{templatePath}");

    //        // InvoiceReportDto invoiceReportDto = data[0] as InvoiceReportDto;

    //        // Assign data to the report
    //        report.RegisterData(data, dataRef);

    //        // Generate the report
    //        report.Prepare();

    //        // Export the report to a PDF format
    //        using (var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport())
    //        {
    //            var exportedStream = new System.IO.MemoryStream();
    //            pdfExport.Export(report, exportedStream);
    //            return exportedStream.ToArray();
    //        }
    //    }
    //}

}
