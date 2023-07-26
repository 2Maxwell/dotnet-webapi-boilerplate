namespace FSH.WebApi.Application.Accounting.Invoices;
public class InvoiceDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int InvoiceIdMandant { get; set; }
    public int? CreditId { get; set; } // Was soll das sein?
    public int? ReservationId { get; set; }
    public int? BookerId { get; set; }
    public int? GuestId { get; set; }
    public int? CompanyId { get; set; }
    public int? CompanyContactId { get; set; }
    public int? TravelAgentId { get; set; }
    public int? TravelAgentContactId { get; set; }
    public DateTime HotelDate { get; set; }
    public DateTime DateCurrent { get; set; }
    public string? InvoiceAddressJson { get; set; } // JSON Format
    public int? InvoiceAddressSource { get; set; } // 0 = Guest, 1 = Company, 2 = TravelAgent, 9 = Manual
    public string? Notes { get; set; }
    public int? State { get; set; }
    public decimal InvoiceTotal { get; set; }
    public decimal InvoiceTotalNet { get; set; }
    public string? InvoiceTaxesJson { get; set; } // JSON Format
    public string? InvoicePaymentsJson { get; set; } // JSON Format
    public int InvoicePosition { get; set; }
    public string? FileName { get; set; }
    public string? InvoiceKz { get; set; } // R = Rechnung, G = Gutschrift, P = Proforma, D = Debitorenrechnung

}
