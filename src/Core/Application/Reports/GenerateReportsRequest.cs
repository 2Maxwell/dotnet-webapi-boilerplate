using FSH.WebApi.Application.Environment.Persons;
using FSH.WebApi.Application.ReportsContract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Reports;
public class GenerateReportsRequest : IRequest<FileContentResult>
{
    public List<PersonAddressReportDto> PersonAddressReportDtos { get; set; }
}

public class GenerateReportsRequestHandler : IRequestHandler<GenerateReportsRequest, FileContentResult>
{
    private const string ReportTemplatePath = "Person2.frx";
    private readonly IReportService<PersonAddressReportDto> _reportService;

    public GenerateReportsRequestHandler(IReportService<PersonAddressReportDto> reportService)
    {
        _reportService = reportService;
    }
    public async Task<FileContentResult> Handle(GenerateReportsRequest request, CancellationToken cancellationToken)
    {
        string dataRef = nameof(PersonAddressReportDto) + "Ref";
        byte[] generatedReport = await _reportService.GenerateReport(ReportTemplatePath, request.PersonAddressReportDtos, dataRef);

        return new FileContentResult(generatedReport, "application/pdf")
        {
            FileDownloadName = "GeneratedReport.pdf"
        };
    }
}
