using FastReport;
using FSH.WebApi.Application.ReportsContract;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace FSH.WebApi.Infrastructure.Reports;
public class ReportService<T> : IReportService<T> where T : class
{
    public async Task<byte[]> GenerateReport(string templatePath, List<T> data, string dataRef)
    {
        using (var report = new Report())
        {
            // Load the report template
            report.Load($"Controllers/Report/TemplateReports/{templatePath}");

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
}
