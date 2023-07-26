using FSH.WebApi.Application.Accounting.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.ReportsContract;
public interface IReportService<T>
    where T : class
{
    Task<byte[]> GenerateReport(string templatePath, List<T> data, string dataRef);
    // Task<byte[]> GenerateReportInvoice(string templatePath, List<T> data, string dataRef);
    Task<byte[]> GenerateReportInvoice(string reportTemplatePath, List<T> invoiceReportDto, string dataRef);
}
