using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Application.Hotel.CancellationPolicys;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Helper;

namespace FSH.WebApi.Application.Accounting.Rates;
public class RateShopMandantDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? DisplayShort { get; set; }
    public string? Display { get; set; }
    public int BookingPolicyId { get; set; }
    public int CancellationPolicyId { get; set; }
    public string? Packages { get; set; } // Value ist Packages.Kz mit , als Trenner
    public string? PackagesValidDisplay { get; set; } // werden nur valide Pacjages bei der Berechnung eingetragen
    public IEnumerable<string> IEPackages
    {
        get
        {
            IEnumerable<string> value = Packages.Split(',', StringSplitOptions.TrimEntries).AsEnumerable();
            return value;
        }
    }

    public string? Categorys { get; set; } // Value ist Category mit , als Trenner
    public decimal RateTotal
    {
        get
        {
            decimal result = 0;
            if (bookingLinesList != null)
            {
                foreach (BookingLine bookingLine in bookingLinesList)
                {
                    result += bookingLine.PriceTotal;
                }
            }

            return result;
        }
    }

    public BookingPolicyDto? bookingPolicyDto { get; set; }
    public CancellationPolicyDto? cancellationPolicyDto { get; set; }
    public List<PackageDto> packagesList { get; set; } = new();
    public List<BookingLine> bookingLinesList { get; set; } = new();
    public List<BookingLineSummary> bookingLineSummaries { get; set; } = new();
    public string? DisplayHighLight { get; set; }
    public string? ConfirmationText { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
    public int? Amount { get; set; }
}
