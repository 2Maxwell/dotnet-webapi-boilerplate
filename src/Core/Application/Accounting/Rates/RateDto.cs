using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Rates;

public class RateDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? DisplayShort { get; set; }
    public string? Display { get; set; }
    public int BookingPolicyId { get; set; }
    public int CancellationPolicyId { get; set; }
    public string? Packages { get; set; } // Value ist Packages.Kz mit , als Trenner
    public IEnumerable<string> IEPackages
    {
        get
        {
            IEnumerable<string> value = Packages.Split(',', StringSplitOptions.TrimEntries).AsEnumerable();
            return value;
        }
    }

    public string? Categorys { get; set; } // Value ist Category mit , als Trenner
    public IEnumerable<string> IECategorys
    {
        get
        {
            IEnumerable<string> value = Categorys.Split(',', StringSplitOptions.TrimEntries).AsEnumerable();
            return value;
        }
    }

    public bool RuleFlex { get; set; }
    public int RateTypeEnumId { get; set; } // Base, Season, Event, Fair
    public int RateScopeEnumId { get; set; } // Public, Private
    public string? DisplayHighLight { get; set; }
    public string? ConfirmationText { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
}
