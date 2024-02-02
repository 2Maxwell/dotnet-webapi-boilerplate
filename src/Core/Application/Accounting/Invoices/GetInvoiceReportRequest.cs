using FSH.WebApi.Domain.Accounting;
using System.Text.Json;

namespace FSH.WebApi.Application.Accounting.Invoices;
public class GetInvoiceReportRequest : IRequest<InvoiceReportDto>
{
    public int MandantId { get; set; }
    public int InvoiceIdMandant { get; set; }
    public int InvoiceId { get; set; }
    public string? ResponseType { get; set; }
}

public class GetInvoiceReportRequestHandler : IRequestHandler<GetInvoiceReportRequest, InvoiceReportDto>
{
    private readonly IReadRepository<Invoice> _repositoryInvoice;
    private readonly IReadRepository<InvoiceDetail> _repositoryInvoiceDetail;

    public GetInvoiceReportRequestHandler(IReadRepository<Invoice> repositoryInvoice, IReadRepository<InvoiceDetail> repositoryInvoiceDetail)
    {
        _repositoryInvoice = repositoryInvoice;
        _repositoryInvoiceDetail = repositoryInvoiceDetail;
    }

    public async Task<InvoiceReportDto> Handle(GetInvoiceReportRequest request, CancellationToken cancellationToken)
    {
        InvoiceReportDto invoiceReportDto = new();
        invoiceReportDto.InvoiceDto = await _repositoryInvoice.GetBySpecAsync((ISpecification<Invoice, InvoiceDto>)new GetInvoiceByInvoiceIdMandantSpec(request), cancellationToken);

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
                    (ISpecification<InvoiceDetail, InvoiceDetailDto>)new GetInvoiceDetailByInvoiceIdMandantSpec(request),
                    cancellationToken);

        List<InvoiceReportDto> invoiceReportDtos = new();
        invoiceReportDtos.Add(invoiceReportDto);

        return invoiceReportDto;
    }
}

public class GetInvoiceByInvoiceIdMandantSpec : Specification<Invoice, InvoiceDto>, ISingleResultSpecification
{
    public GetInvoiceByInvoiceIdMandantSpec(GetInvoiceReportRequest request) =>
        Query.Where(i => i.MandantId == request.MandantId && i.InvoiceIdMandant == request.InvoiceIdMandant && i.Id == request.InvoiceId);
}

public class GetInvoiceDetailByInvoiceIdMandantSpec : Specification<InvoiceDetail, InvoiceDetailDto>
{
    public GetInvoiceDetailByInvoiceIdMandantSpec(GetInvoiceReportRequest request) =>
        Query.Where(i => i.MandantId == request.MandantId && i.InvoiceIdMandant == request.InvoiceIdMandant && i.InvoiceId == request.InvoiceId)
        .OrderBy(i => i.BookingDate)
        .ThenBy(i => i.ItemNumber);
}