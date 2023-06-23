namespace FSH.WebApi.Application.Hotel.Rooms;

public class RoomSelectDto : IDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public int Beds { get; set; } = 1;
    public int BedsExtra { get; set; }
    public bool Clean { get; set; }
}
