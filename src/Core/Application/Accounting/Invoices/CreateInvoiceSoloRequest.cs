﻿using FSH.WebApi.Application.ReportsContract;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Accounting.Invoices;
public class CreateInvoiceSoloRequest : IRequest<string>
{
    public int MandantId { get; set; }
    public int InvoiceIdMandant { get; set; }
    public int? CreditId { get; set; } // Was soll das sein? Soll Zahlart sein. Obsolete da InvoicePaymentJson
    public int? ReservationId { get; set; }
    public int? BookerId { get; set; }
    public int? GuestId { get; set; }
    public int? CompanyId { get; set; }
    public int? CompanyContactId { get; set; }
    public int? TravelAgentId { get; set; }
    public int? TravelAgentContactId { get; set; }
    public DateTime HotelDate { get; set; }
    public string? InvoiceAddressJson { get; set; } // JSON Format
    public int? InvoiceAddressSource { get; set; } // 0 = Guest, 1 = Company, 2 = TravelAgent, 9 = Manual
    public string? Notes { get; set; }
    public decimal InvoiceTotal { get; set; }
    public decimal InvoiceTotalNet { get; set; }
    public string? InvoiceTaxesJson { get; set; } // JSON Format
    public string? InvoicePaymentsJson { get; set; } // JSON Format
    public string? InvoiceKz { get; set; } // R = Rechnung, G = Gutschrift, P = Proforma, D = Debitorenrechnung
    public bool PrintInvoice { get; set; }
}

public class CreateInvoiceSoloRequestValidator : CustomValidator<CreateInvoiceSoloRequest>
{
    public CreateInvoiceSoloRequestValidator()
    {
        RuleFor(x => x.MandantId)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.HotelDate)
            .NotEmpty();
        //RuleFor(x => x.InvoiceIdMandant)
        //    .NotEmpty()
        //    .GreaterThan(0);
        RuleFor(x => x.InvoiceKz)
            .NotEmpty();
    }
}

public class CreateInvoiceSoloRequestHandler : IRequestHandler<CreateInvoiceSoloRequest, string>
{
    private readonly IReadRepository<Invoice> _invoiceRepository;
    private readonly IRepository<Invoice> _invoiceRepository1;
    private readonly IReadRepository<InvoiceDetail> _invoiceDetailRepository;
    private readonly IRepository<MandantNumbers> _mandantNumbersRepository;
    private readonly IReportService<InvoiceReportDto> _reportService;
    private readonly IReadRepository<MandantDetail> _repositoryMandantDetail;
    private readonly IReadRepository<Reservation> _repositoryReservation;

    public CreateInvoiceSoloRequestHandler(IReadRepository<Invoice> invoiceRepository, IRepository<Invoice> invoiceRepository1, IReadRepository<InvoiceDetail> invoiceDetailRepository, IRepository<MandantNumbers> mandantNumbersRepository, IReportService<InvoiceReportDto> reportService, IReadRepository<MandantDetail> repositoryMandantDetail, IReadRepository<Reservation> repositoryReservation)
    {
        _invoiceRepository = invoiceRepository;
        _invoiceRepository1 = invoiceRepository1;
        _invoiceDetailRepository = invoiceDetailRepository;
        _mandantNumbersRepository = mandantNumbersRepository;
        _reportService = reportService;
        _repositoryMandantDetail = repositoryMandantDetail;
        _repositoryReservation = repositoryReservation;
    }

    public async Task<string> Handle(CreateInvoiceSoloRequest request, CancellationToken cancellationToken)
    {
        List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();

        var invoice = new Invoice(
                request.MandantId,
                request.InvoiceIdMandant,
                request.CreditId,
                request.ReservationId,
                request.BookerId,
                request.GuestId,
                request.CompanyId,
                request.CompanyContactId,
                request.TravelAgentId,
                request.TravelAgentContactId,
                request.HotelDate,
                DateTime.Now,
                request.InvoiceAddressJson,
                request.InvoiceAddressSource,
                request.Notes,
                1, // request.State,
                request.InvoiceTotal, // wenn es beim Client ausgerechnet wird
                request.InvoiceTotalNet,
                request.InvoiceTaxesJson,
                request.InvoicePaymentsJson,
                null, // request.FileName, // ist ein wenig tricky wann ds ermittelt wird und wie gespeichert wird
                request.InvoiceKz);

        invoice.DomainEvents.Add(EntityCreatedEvent.WithEntity(invoice));
        var invoiceSaved = await _invoiceRepository1.AddAsync(invoice, cancellationToken);

        GetInvoiceReportRequest getInvoiceReportRequest = new();
        GetInvoiceReportRequestHandler getInvoiceReportRequestHandler = new(_invoiceRepository, _invoiceDetailRepository);
        var resultTemp = await getInvoiceReportRequestHandler.Handle(getInvoiceReportRequest, cancellationToken);

        string result = $"{invoice.Id}-{invoice.InvoiceIdMandant}";

        return result;
    }
}

