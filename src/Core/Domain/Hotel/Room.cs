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
    public bool ArrExp { get; set; }
    public bool ArrCi { get; set; }
    public bool Occ { get; set; }
    public bool DepExp { get; set; }
    public bool DepOut { get; set; }

    //Status für Zimmerreinigung
    public bool Assigned { get; set; }
    public int DirtyDays { get; set; }
    public int AssignedId { get; set; } // Zimmermädchen zugewiesen
    public bool IsCleaning { get; set; }
    public bool PendingClean { get; set; }
    public bool CleanChecked { get; set; }
    public int MinutesOccupied { get; set; }
    public int MinutesDeparture { get; set; }
    public int MinutesDefault { get; set; }

    // TODO Zusätzliche Anlagen z.B. Gästetoilette, Flur, Treppenhaus, Empfangshalle anlegen können um
    // diese bei Reinigungsarbeiten zuteilen zu können.

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

    public Room SetRoomStateCheckIn(bool arrivalExpected, bool arrivalCheckIn, bool occupied, bool clean)
    {
        ArrExp = arrivalExpected;
        ArrCi = arrivalCheckIn;
        Occ = occupied;
        Clean = clean;

        return this;
    }

    public Room SetRoomStateCheckOut(bool occupied, bool departureExpected, bool departureOut)
    {
        Occ = occupied;
        DepExp = departureExpected;
        DepOut = departureOut;
        return this;
    }

    public Room SetRoomStateNightAuditOccupied(DateOnly hotelDate, DateOnly arrivalDate, DateOnly departureDate)
    {
        // nur für Reservierungen die im Haus sind
        if (hotelDate == arrivalDate)
        {
            ArrExp = false;
            ArrCi = false;
            Occ = true;
            DepExp = false;
            DepOut = false;
            Clean = false;
            DirtyDays = DirtyDays++;
            Assigned = false;
            IsCleaning = false;
            PendingClean = false;
            CleanChecked = false;
        }

        if (hotelDate == departureDate.AddDays(-1))
        {
            DepExp = true;
            DepOut = false;
        }

        return this;
    }

    public Room SetRoomStateArrivalExpected()
    {
        ArrExp = true;
        ArrCi = false;
        return this;
    }

    public Room SetRoomStateAssignToCleaning(bool assigned, int assignedId)
    {
        Assigned = assigned;
        AssignedId = assignedId;
        return this;
    }

    public Room SetRoomStateIsCleaning(bool isCleaning)
    {
        IsCleaning = isCleaning;
        return this;
    }

    public Room SetRoomStatePendingClean(bool pendingClean)
    {
        PendingClean = pendingClean;
        return this;
    }

    public Room SetRoomStateCleanChecked(bool cleanChecked)
    {
        CleanChecked = cleanChecked;
        Clean = cleanChecked ? true : false;
        return this;
    }

    public Room SetRoomStateCleanDirty(bool clean)
    {
        Clean = clean;
        return this;
    }

}
