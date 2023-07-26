using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Domain.Accounting;
public class Invoice : AuditableEntity<int>, IAggregateRoot
{
    public Invoice(int mandantId, int invoiceIdMandant, int? creditId, int? reservationId, int? bookerId, int? guestId, int? companyId, int? companyContactId, int? travelAgentId, int? travelAgentContactId, DateTime hotelDate, DateTime dateCurrent, string? invoiceAddressJson, int? invoiceAddressSource, string? notes, int? state, decimal invoiceTotal, decimal invoiceTotalNet, string? invoiceTaxesJson, string? invoicePaymentsJson, string? fileName, string? invoiceKz)
    {
        MandantId = mandantId;
        InvoiceIdMandant = invoiceIdMandant;
        CreditId = creditId;
        ReservationId = reservationId;
        BookerId = bookerId;
        GuestId = guestId;
        CompanyId = companyId;
        CompanyContactId = companyContactId;
        TravelAgentId = travelAgentId;
        TravelAgentContactId = travelAgentContactId;
        HotelDate = hotelDate;
        DateCurrent = dateCurrent;
        InvoiceAddressJson = invoiceAddressJson;
        InvoiceAddressSource = invoiceAddressSource;
        Notes = notes;
        State = state;
        InvoiceTotal = invoiceTotal;
        InvoiceTotalNet = invoiceTotalNet;
        InvoiceTaxesJson = invoiceTaxesJson;
        InvoicePaymentsJson = invoicePaymentsJson;
        FileName = fileName;
        InvoiceKz = invoiceKz;
    }

    [Required]
    public int MandantId { get; set; }
    [Required]
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
    [StringLength(300)]
    public string? InvoiceAddressJson { get; set; } // JSON Format
    public int? InvoiceAddressSource { get; set; } // 0 = Manual, 1 = Guest, 2 = Company, 3 = TravelAgent
    [StringLength(100)]
    public string? Notes { get; set; }
    public int? State { get; set; }
    public decimal InvoiceTotal { get; set; }
    public decimal InvoiceTotalNet { get; set; }
    [StringLength(200)]
    public string? InvoiceTaxesJson { get; set; } // JSON Format
    [StringLength(1000)]
    public string? InvoicePaymentsJson { get; set; } // JSON Format
    [StringLength(300)]
    public string? FileName { get; set; }
    [StringLength(1)]
    public string? InvoiceKz { get; set; } // R = Rechnung, G = Gutschrift, P = Proforma, D = Debitorenrechnung

}

