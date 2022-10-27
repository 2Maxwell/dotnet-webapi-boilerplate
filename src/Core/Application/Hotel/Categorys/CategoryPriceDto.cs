using FSH.WebApi.Application.Hotel.PriceCats;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class CategoryPriceDto : IDto
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Kz { get; set; }
    public string? Name { get; set; }
    public int NumberOfBeds { get; set; }
    public int NumberOfExtraBeds { get; set; }
    public int BedsCount
    {
        get
        {
            int value;
            value = NumberOfBeds + NumberOfExtraBeds;
            return value;

        }
    }

    public List<PriceCatDto>? PriceCats { get; set; }

}