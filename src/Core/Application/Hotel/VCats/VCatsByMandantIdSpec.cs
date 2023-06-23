using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.VCats;

public class VCatsByMandantIdSpec : Specification<VCat>
{
    public VCatsByMandantIdSpec(int mandantId, DateTime hoteldate)
    {
        Query.Where(c => c.MandantId == mandantId && c.Date >= hoteldate.Date);
    }
}