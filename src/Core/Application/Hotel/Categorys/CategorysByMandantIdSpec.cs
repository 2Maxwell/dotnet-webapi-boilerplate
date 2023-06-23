using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Categorys;
public class CategorysByMandantIdSpec : Specification<Category>
{
    public CategorysByMandantIdSpec(int mandantId) =>
                Query.Where(c => c.MandantId == mandantId);
}
