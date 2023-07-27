using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Application.Hotel.Reservations;
using FSH.WebApi.Application.ReportsContract;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Hotel;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSH.WebApi.Application.Accounting.Invoices;
public class GetInvoiceReportRequest : IRequest<FileContentResult>
{
    public int InvoiceIdMandant { get; set; }
    //public Invoice Invoice { get; set; }
    //public List<InvoiceDetail> InvoiceDetails { get; set; }
    //public bool PrintInvoice { get; set; }
}

public class GetInvoiceReportRequestHandler : IRequestHandler<GetInvoiceReportRequest, FileContentResult>
{
    private const string ReportTemplatePath = "invoiceReportAutoBuild_2.frx";
    private readonly IReadRepository<Invoice> _repositoryInvoice;
    private readonly IReadRepository<InvoiceDetail> _repositoryInvoiceDetail;
    private readonly IReportService<InvoiceReportDto> _reportService;
    private readonly IReadRepository<MandantDetail> _repositoryMandantDetail;
    private readonly IReadRepository<Reservation> _repositoryReservation;

    public GetInvoiceReportRequestHandler(IReadRepository<Invoice> repositoryInvoice, IReadRepository<InvoiceDetail> repositoryInvoiceDetail, IReportService<InvoiceReportDto> reportService, IReadRepository<MandantDetail> repositoryMandantDetail, IReadRepository<Reservation> repositoryReservation)
    {
        _repositoryInvoice = repositoryInvoice;
        _repositoryInvoiceDetail = repositoryInvoiceDetail;
        _reportService = reportService;
        _repositoryMandantDetail = repositoryMandantDetail;
        _repositoryReservation = repositoryReservation;
    }

    public async Task<FileContentResult> Handle(GetInvoiceReportRequest request, CancellationToken cancellationToken)
    {
        InvoiceReportDto invoiceReportDto = new();
        invoiceReportDto.InvoiceDto = await _repositoryInvoice.GetBySpecAsync((ISpecification<Invoice, InvoiceDto>)new GetInvoiceByInvoiceIdMandantSpec(request.InvoiceIdMandant), cancellationToken);

        if (invoiceReportDto.InvoiceDto == null)
        {
            throw new InvalidOperationException($"Unable to retrieve invoice with id {request.InvoiceIdMandant}");
        }

        invoiceReportDto.InvoiceAddressDto = invoiceReportDto.InvoiceDto.InvoiceAddressJson is null
                                              ? new InvoiceAddressDto()
                                              : JsonSerializer.Deserialize<InvoiceAddressDto>(invoiceReportDto.InvoiceDto.InvoiceAddressJson);

        invoiceReportDto.InvoiceTaxDtos = invoiceReportDto.InvoiceDto.InvoiceTaxesJson is null
                                              ? new List<InvoiceTaxDto>()
                                              : JsonSerializer.Deserialize<List<InvoiceTaxDto>>(invoiceReportDto.InvoiceDto.InvoiceTaxesJson);

        invoiceReportDto.InvoicePaymentDtos = invoiceReportDto.InvoiceDto.InvoicePaymentsJson is null
                                              ? new List<InvoicePaymentDto>()
                                              : JsonSerializer.Deserialize<List<InvoicePaymentDto>>(invoiceReportDto.InvoiceDto.InvoicePaymentsJson);

        invoiceReportDto.InvoiceDetails = await _repositoryInvoiceDetail.ListAsync(
                    (ISpecification<InvoiceDetail, InvoiceDetailDto>)new GetInvoiceDetailByInvoiceIdMandantSpec(request.InvoiceIdMandant),
                    cancellationToken);

        invoiceReportDto.MandantDetailDto = await _repositoryMandantDetail.GetBySpecAsync(
                    (ISpecification<MandantDetail, MandantDetailDto>)new GetMandantDetailByIdSpec(invoiceReportDto.InvoiceDto.MandantId),
                    cancellationToken);

        invoiceReportDto.ReservationDto = invoiceReportDto.InvoiceDto.ReservationId is null
                                              ? new ReservationInvoiceReportDto()
                                              : (await _repositoryReservation.GetByIdAsync(invoiceReportDto.InvoiceDto.ReservationId.Value, cancellationToken)).Adapt<ReservationInvoiceReportDto>();

        List<InvoiceReportDto> invoiceReportDtos = new();
        invoiceReportDtos.Add(invoiceReportDto);

        string dataRef = "invoiceReportDto"; // nameof(InvoiceReportDto);

        byte[] generatedReport = await _reportService.GenerateReportInvoice(ReportTemplatePath, invoiceReportDtos, dataRef);

        return new FileContentResult(generatedReport, "application/pdf")
        {
            FileDownloadName = "GeneratedReport.pdf"
        };
    }

}

public class GetInvoiceByInvoiceIdMandantSpec : Specification<Invoice, InvoiceDto>, ISingleResultSpecification
{
    public GetInvoiceByInvoiceIdMandantSpec(int invoiceIdMandant) =>
        Query.Where(i => i.InvoiceIdMandant == invoiceIdMandant);
}

public class GetInvoiceDetailByInvoiceIdMandantSpec : Specification<InvoiceDetail, InvoiceDetailDto>
{
    public GetInvoiceDetailByInvoiceIdMandantSpec(int invoiceIdMandant) =>
        Query.Where(i => i.InvoiceIdMandant == invoiceIdMandant);
}