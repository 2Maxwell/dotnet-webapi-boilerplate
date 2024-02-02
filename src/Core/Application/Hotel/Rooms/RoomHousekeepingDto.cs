namespace FSH.WebApi.Application.Hotel.Rooms;
public class RoomHousekeepingDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public int Beds { get; set; }
    public int BedsExtra { get; set; }
    public int BedsMax { get; set; }
    public bool Clean { get; set; }
    public bool Blocked { get; set; }
    public DateTime? BlockedStart { get; set; }
    public DateTime? BlockedEnd { get; set; }
    public bool ArrExp { get; set; }
    public bool ArrCi { get; set; }
    public bool Occ { get; set; }
    public bool DepExp { get; set; }
    public bool DepOut { get; set; }
    public int CleaningState { get; set; }
    public int MinutesOccupied { get; set; }
    public int MinutesDeparture { get; set; }
    public int MinutesDefault { get; set; }
    public int AssignedId { get; set; }
    public int DirtyDays { get; set; }
}
