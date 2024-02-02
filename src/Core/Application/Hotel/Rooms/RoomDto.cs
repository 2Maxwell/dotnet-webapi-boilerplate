namespace FSH.WebApi.Application.Hotel.Rooms;

public class RoomDto : IDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? DisplayDescription { get; set; }
    public int Beds { get; set; } = 1;
    public int BedsExtra { get; set; }
    public string? Facilities { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Clean { get; set; }
    public bool Blocked { get; set; }
    public DateTime? BlockedStart { get; set; }
    public DateTime? BlockedEnd { get; set; }
    public string? CategoryName { get; set; }
    public int CleaningState { get; set; }
    public int DirtyDays { get; set; }
    public int AssignedId { get; set; }
    public int MinutesOccupied { get; set; }
    public int MinutesDeparture { get; set; }
    public int MinutesDefault { get; set; }
}
