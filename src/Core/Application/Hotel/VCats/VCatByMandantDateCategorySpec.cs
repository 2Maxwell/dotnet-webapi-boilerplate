using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.VCats;
public class VCatByMandantDateCategorySpec : Specification<VCat>, ISingleResultSpecification
{
    public VCatByMandantDateCategorySpec(int mandantId, DateTime date, int categoryId)
    {
        Query.Where(c => c.MandantId == mandantId && c.Date == date.Date && c.CategoryId == categoryId);
    }
}
