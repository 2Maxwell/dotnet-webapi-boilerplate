using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class GetCategorySelectCatPaxRequest : IRequest<List<CategorySelectCatPaxDto>>
{
    public int MandantId { get; set; }
    public GetCategorySelectCatPaxRequest(int mandantId) => MandantId = mandantId;
}

public class CategoryByMandantIdSpec : Specification<Category, CategoryDto>
{
    public CategoryByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId)
             .OrderBy(c => c.OrderNumber);

        // .Where(c => c.MandantId == mandantId)
    }
}

public class GetCategorySelectCatPaxRequestHandler : IRequestHandler<GetCategorySelectCatPaxRequest, List<CategorySelectCatPaxDto>>
{
    private readonly IRepository<Category> _repository;

    public GetCategorySelectCatPaxRequestHandler(IRepository<Category> repository) =>
        _repository = repository;

    public async Task<List<CategorySelectCatPaxDto>> Handle(GetCategorySelectCatPaxRequest request, CancellationToken cancellationToken)
    {
        List<CategoryDto> listedb = await _repository.ListAsync((ISpecification<Category, CategoryDto>)new CategoryByMandantIdSpec(request.MandantId), cancellationToken);

        List<CategorySelectCatPaxDto> listeOut = new();
        int counter = 1;
        foreach (CategoryDto item in listedb)
        {
            string? cat = item.Kz;
            int maxPax = item.NumberOfExtraBeds + item.NumberOfBeds;

            for (int i = 1; i <= maxPax; i++)
            {
                CategorySelectCatPaxDto catDto = new();
                catDto.Kz = item.Kz;
                catDto.NumberOfBeds = i;
                catDto.NumberOfExtraBeds = counter;
                catDto.CatPax = item.Kz + "_Pax(" + i + ")";
                listeOut.Add(catDto);
                counter++;
            }
        }

        return listeOut;
    }
}