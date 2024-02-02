using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Accounting;

public class Mandant : AuditableEntity<int>, IAggregateRoot
{
    [StringLength(100)]
    public string? Name { get; set; }
    [Required]
    [StringLength(10)]
    public string? Kz { get; set; }
    [Required]
    public int GroupMember { get; set; }
    public bool GroupHead { get; set; } = false;
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime HotelDate { get; set; }
    // public int TaxCountryId { get; set; }

    public Mandant()
    {

    }

    public Mandant(string? name, string? kz, int groupMember, bool groupHead)
    {
        Name = name;
        Kz = kz;
        GroupMember = groupMember;
        GroupHead = groupHead;
    }

    public Mandant Update(string? name, string? kz, int groupMember, bool groupHead)
    {
        Name = name;
        Kz = kz;
        GroupMember = groupMember;
        GroupHead = groupHead;
        return this;
    }

    public Mandant UpdateHotelDate(DateTime hotelDate)
    {
        HotelDate = hotelDate;
        return this;
    }

}
