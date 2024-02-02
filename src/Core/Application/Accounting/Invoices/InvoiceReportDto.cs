namespace FSH.WebApi.Application.Accounting.Invoices;
public class InvoiceReportDto : IDto
{
    public InvoiceDto? InvoiceDto { get; set; }
    public List<InvoiceDetailDto>? InvoiceDetails { get; set; }
    // public MandantDetailDto? MandantDetailDto { get; set; }
    // public ReservationInvoiceReportDto? ReservationDto { get; set; }
    public InvoiceAddressDto? InvoiceAddressDto { get; set; }
    public List<InvoiceTaxDto>? InvoiceTaxDtos { get; set; }
    public List<InvoicePaymentDto>? InvoicePaymentDtos { get; set; }
}

public class InvoiceAddressDto : IDto
{
    public string? Name1 { get; set; }
    public string? Name2 { get; set; }
    public string? ContactName { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Email { get; set; }
    public bool SendEmail { get; set; }
}

public class InvoiceTaxDto : IDto
{
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal TotalAmount { get; set; }
}

public class InvoicePaymentDto : IDto
 {
    public DateTime HotelDate { get; set; }
    public string? Name { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public bool Debit { get; set; }
    public int ItemId { get; set; }
    public int ItemNumber { get; set; }
    public int TaxId { get; set; }
    public decimal TaxRate { get; set; }
    public int? KasseId { get; set; }
}

