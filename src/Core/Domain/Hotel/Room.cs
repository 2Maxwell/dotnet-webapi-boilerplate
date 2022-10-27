using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.WebApi.Domain.Hotel;

public class Room : AuditableEntity<int>, IAggregateRoot
{
    [Required]
    public int MandantId { get; set; }
    [Required]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public string NameUnique { get; set; }

    [Required]
    [StringLength(100)]
    public string? Description { get; set; }

    [Required]
    [StringLength(500)]
    public string? DisplayDescription { get; set; }

    public int Beds { get; set; } = 1;
    public int BedsExtra { get; set; }

    [NotMapped]
    public int BedsMax
    {
        get
        {
            return Beds + BedsExtra;
        }
    }

    [StringLength(500)]
    public string? Facilities { get; set; }
    public bool Clean { get; set; }
    public bool Blocked { get; set; }
    public DateTime? BlockedStart { get; set; }
    public DateTime? BlockedEnd { get; set; }

    [StringLength(50)]
    public string? PhoneNumber { get; set; }
    public string? PhoneNumberUnique { get; set; }

    public virtual Category Category { get; private set; } = default!;

    public Room()
    {

    }

    public Room(int mandantId, int categoryId, string name, string description, string displayDescription, int beds, int bedsExtra, string facilities, string phoneNumber)
    {
        MandantId = mandantId;
        CategoryId = categoryId;
        Name = name;
        Description = description;
        DisplayDescription = displayDescription;
        Beds = beds;
        BedsExtra = bedsExtra;
        Facilities = facilities;
        PhoneNumber = phoneNumber;
        Clean = false;
        Blocked = true;
        BlockedStart = null;
        BlockedEnd = null;
        NameUnique = Name + "|" + MandantId;
        PhoneNumberUnique = this.PhoneNumber + "|" + this.MandantId;
    }

    public Room Update(int categoryId, string name, string? description, string displayDescription,
        int beds, int bedsExtra, string facilities, string phoneNumber, bool clean, bool blocked,
        DateTime? blockedStart, DateTime? blockedEnd)
    {
        CategoryId = categoryId;
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (displayDescription is not null && DisplayDescription?.Equals(displayDescription) is not true) DisplayDescription = displayDescription;
        Beds = beds;
        BedsExtra = bedsExtra;
        if (facilities is not null && Facilities?.Equals(facilities) is not true) Facilities = facilities;
        if (phoneNumber is not null && PhoneNumber?.Equals(phoneNumber) is not true) PhoneNumber = phoneNumber;
        Clean = clean;
        Blocked = blocked;
        if (blockedStart is not null && BlockedStart?.Equals(blockedStart) is not true) BlockedStart = blockedStart;
        if (blockedEnd is not null && BlockedEnd?.Equals(blockedEnd) is not true) BlockedEnd = blockedEnd;
        NameUnique = Name + "|" + MandantId;
        PhoneNumberUnique = this.PhoneNumber + "|" + this.MandantId;

        return this;
    }


}
