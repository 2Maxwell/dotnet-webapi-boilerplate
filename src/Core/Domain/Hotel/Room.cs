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
    public bool ArrExp { get; set; }
    public bool ArrCi { get; set; }
    public bool Occ { get; set; }
    public bool DepExp { get; set; }
    public bool DepOut { get; set; }
    public int CleaningState { get; set; }

    // 0 = nicht zugewiesen UnAssigned
    // 1 = zugewiesen Assigned
    // 2 = in Reinigung IsCleaning
    // 3 = Reinigung abgeschlossen PendingClean
    // 4 = Reinigung abgeschlossen und kontrolliert CleanChecked

    // Status für Zimmerreinigung
    // public bool Assigned { get; set; }
    public int DirtyDays { get; set; }

    public int AssignedId { get; set; } // Zimmermädchen zugewiesen

    // public bool IsCleaning { get; set; }
    // public bool PendingClean { get; set; }
    // public bool CleanChecked { get; set; }
    public int MinutesOccupied { get; set; }
    public int MinutesDeparture { get; set; }
    public int MinutesDefault { get; set; }

    // TODO Zusätzliche Anlagen z.B. Gästetoilette, Flur, Treppenhaus, Empfangshalle anlegen können um
    // diese bei Reinigungsarbeiten zuteilen zu können.

    public virtual Category Category { get; private set; } = default!;

    public Room()
    {

    }

    public Room(int mandantId, int categoryId, string name, string description, string displayDescription, int beds, int bedsExtra, string facilities, string phoneNumber, int cleaningState, int dirtyDays, int assignedId,
        int minutesOccupied, int minutesDeparture, int minutesDefault)
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
        CleaningState = cleaningState;
        DirtyDays = dirtyDays;
        AssignedId = assignedId;
        MinutesOccupied = minutesOccupied;
        MinutesDeparture = minutesDeparture;
        MinutesDefault = minutesDefault;

    }

    public Room Update(int categoryId, string name, string? description, string displayDescription,
        int beds, int bedsExtra, string facilities, string phoneNumber, bool clean, bool blocked,
        DateTime? blockedStart, DateTime? blockedEnd, int cleaningState, int dirtyDays,int assignedId,
        int minutesOccupied, int minutesDeparture, int minutesDefault)
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
        CleaningState = cleaningState;
        DirtyDays = dirtyDays;
        AssignedId = assignedId;
        MinutesOccupied = minutesOccupied;
        MinutesDeparture = minutesDeparture;
        MinutesDefault = minutesDefault;
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
            CleaningState = 0;
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

    public Room SetRoomStateAssignToCleaning(int assignedId)
    {
        CleaningState = 1; // 1 = zugewiesen
        AssignedId = assignedId;
        return this;
    }

    public Room SetRoomStateIsCleaning()
    {
        CleaningState = 2; // 2 = IsCleaning
        return this;
    }

    public Room SetRoomStatePendingClean()
    {
        CleaningState = 3; // 3 = PendingClean
        return this;
    }

    public Room SetRoomStateCleanChecked()
    {
        CleaningState = 4; // 4 = CleanChecked
        Clean = true;
        return this;
    }

    public Room SetRoomStateCleanDirty(bool clean)
    {
        Clean = clean;
        return this;
    }
}