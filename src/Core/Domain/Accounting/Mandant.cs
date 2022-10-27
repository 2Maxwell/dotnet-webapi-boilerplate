using System.ComponentModel.DataAnnotations;

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

}


