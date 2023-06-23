using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FSH.WebApi.Domain.Environment;
public class VipStatus : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    [StringLength(100)]
    public string? Name { get; set; }
    [StringLength(10)]
    public string? Kz { get; set; }
    [StringLength(150)]
    public string? Arrival { get; set; }
    [StringLength(150)]
    public string? Daily { get; set; }
    [StringLength(150)]
    public string? Departure { get; set; }
    [StringLength(100)]
    public string? ChipIcon { get; set; }
    [StringLength(50)]
    public string? ChipText { get; set; }

    public VipStatus(int mandantId, string? name, string? kz, string? arrival, string? daily, string? departure, string? chipIcon, string? chipText)
    {
        MandantId = mandantId;
        Name = name;
        Kz = kz;
        Arrival = arrival;
        Daily = daily;
        Departure = departure;
        ChipIcon = chipIcon;
        ChipText = chipText;
    }

    public VipStatus Update(string? name, string? kz, string? arrival, string? daily, string? departure, string? chipIcon, string? chipText)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (kz is not null && Kz?.Equals(kz) is not true) Kz = kz;
        if (arrival is not null && Arrival?.Equals(arrival) is not true) Arrival = arrival;
        if (daily is not null && Daily?.Equals(daily) is not true) Daily = daily;
        if (departure is not null && Departure?.Equals(departure) is not true) Departure = departure;
        if (chipIcon is not null && ChipIcon?.Equals(chipIcon) is not true) ChipIcon = chipIcon;
        if (chipText is not null && ChipText?.Equals(chipText) is not true) ChipText = chipText;
        return this;
    }

}
