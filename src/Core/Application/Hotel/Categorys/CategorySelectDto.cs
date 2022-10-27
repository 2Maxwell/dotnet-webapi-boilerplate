namespace FSH.WebApi.Application.Hotel.Categorys;

public class CategorySelectDto : IDto
{
    public int Id { get; set; }
    public string? Kz { get; set; }
    public string? Name { get; set; }
}
