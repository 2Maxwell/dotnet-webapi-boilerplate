using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Domain.Helper;
public class BookingLineSummary
{
    public DateTime Date
    {
        get
        {
            DateTime date = Convert.ToDateTime(SourceList.Select(x => x.DateBooking).First());
            return date;
        }
    }

    public decimal? Amount
    {
        get
        {
            decimal value = SourceList.Select(x => x.Amount).First();
            return value;
        }
    }

    public decimal Price
    {
        get
        {
            decimal value = SourceList.Sum(x => x.Price);
            return value;
        }
    }

    public decimal Total
    {
        get
        {
            decimal value = Amount != null ? Price * Convert.ToDecimal(Amount) : Price;
            return value;
        }
    }

    public string Description
    {
        get
        {
            string value = SourceList.Select(x => x.Name).First();
            return value;
        }
    }

    public string ReferenceId
    {
        get
        {
            string value = SourceList.Count > 0 ? SourceList[0].BookingLineNumberId + SourceList[0].Source : string.Empty;
            return value;
        }
    }

    public int InvoicePosition
    {
        get
        {
            int value = SourceList.Select(x => x.InvoicePos).First();
            return (int)value;
        }
    }

    public string TaxLine
    {
        get
        {
            string value = string.Empty;
            var taxesGroupedByTaxRate = SourceList.GroupBy(x => x.TaxRate);
            foreach(var group in taxesGroupedByTaxRate)
            {
                value += "Tax: " + group.Key + "% T: " + (group.Sum(x => x.PriceTotal) / (100 + group.Key) * group.Key).ToString("N2") + " N: " + (group.Sum(x => x.PriceTotal) / (100 + group.Key) * 100).ToString("N2") + " Total: " + group.Sum(x => x.PriceTotal).ToString("N2") + " ";
            }

            return value;
        }
    }

    public decimal BruttoByTaxRate(decimal taxRate)
    {
        decimal value = SourceList.Where(x => x.TaxRate == taxRate).Sum(x => x.PriceTotal);
        return value;
    }

    public decimal NettoByTaxRate(decimal taxRate)
    {
        decimal value = SourceList.Where(x => x.TaxRate == taxRate).Sum(x => x.PriceTotal) / (100 + taxRate) * 100;
        return value;
    }

    public decimal TaxByTaxRate(decimal taxRate)
    {
        decimal value = SourceList.Where(x => x.TaxRate == taxRate).Sum(x => x.PriceTotal) / (100 + taxRate) * taxRate;
        return value;
    }

    public bool Debit
    {
        get
        {
            bool value = SourceList.Select(x => x.Debit).First();
            return value;
        }
    }

    public List<BookingLine> SourceList { get; set; } = new();
}

public class BookingLineSummaryReportResult
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public string Description { get; set; }
    public string ReferenceId { get; set; }
    public int InvoicePosition { get; set; }
    public string TaxLine { get; set; }
    public bool Debit { get; set; }
}