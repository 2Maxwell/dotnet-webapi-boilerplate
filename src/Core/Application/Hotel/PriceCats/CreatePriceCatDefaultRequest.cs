using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PriceCats;
public class CreatePriceCatDefaultRequest : IRequest<bool>
{
    public int MandantId { get; set; }

    public DateTime EndDate { get; set; }
}

public class CreatePriceCatDefaultRequestValidator : CustomValidator<CreatePriceCatDefaultRequest>
{
    public CreatePriceCatDefaultRequestValidator(IReadRepository<Category> repository, IStringLocalizer<CreateCategoryRequestValidator> localizer)
    {
        RuleFor(x => x.EndDate)
        .NotEmpty();
        // .GreaterThanOrEqualTo(DateTime.Today);
        //TODO für Produktion wieder eintragen
    }
}

public class PriceCatLastDateCurrentSpec : Specification<PriceCat, DateTime>, ISingleResultSpecification
{
    public PriceCatLastDateCurrentSpec(int mandantId) =>
        Query.Where(x => x.DatePrice >= DateTime.Today.AddDays(-200) && (x.MandantId == mandantId))
        .OrderByDescending(x => x.DatePrice);
    // für Produktion AddDays korrigieren

}

public class CreatePriceCatDefaultRequstHandler : IRequestHandler<CreatePriceCatDefaultRequest, bool>
{
    private readonly IReadRepository<Category> _repositoryCategory;
    private readonly IRepositoryWithEvents<PriceCat> _repositoryPriceCat;

    public CreatePriceCatDefaultRequstHandler(IReadRepository<Category> repositoryCategory, IRepositoryWithEvents<PriceCat> repository)
    {
        _repositoryCategory = repositoryCategory;
        _repositoryPriceCat = repository;
    }

    public async Task<bool> Handle(CreatePriceCatDefaultRequest request, CancellationToken cancellationToken)
    {
        var specCategory = new CategoryByMandantIdSpec(request.MandantId);
        List<CategoryDto> listCategoryDto = await _repositoryCategory.ListAsync(specCategory, cancellationToken);

        var specLastDate = new PriceCatLastDateCurrentSpec(request.MandantId);
        PriceCat prcat = await _repositoryPriceCat.GetBySpecAsync(specLastDate, cancellationToken);

        foreach (CategoryDto cDto in listCategoryDto)
        {
            DateOnly start = DateOnly.FromDateTime(prcat.DatePrice.AddDays(1));
            DateOnly end = DateOnly.FromDateTime(request.EndDate);

            int beds = 1;
            while (beds <= (cDto.NumberOfBeds + cDto.NumberOfExtraBeds))
            {
                DateOnly aktDate = start;
                while (aktDate <= end)
                {
                    PriceCat pc = new PriceCat(
                        cDto.MandantId,
                        cDto.Id,
                        aktDate.ToDateTime(TimeOnly.Parse("12:00 PM")),
                        beds,
                        9990 + beds,
                        9990 + beds,
                        0,
                        string.Empty,
                        (int)RateTypeEnum.Base);

                    pc.DomainEvents.Add(EntityCreatedEvent.WithEntity(pc));
                    await _repositoryPriceCat.AddAsync(pc, cancellationToken);

                    aktDate = aktDate.AddDays(1);
                }

                beds++;
            }
        }

        return true;

    }
}
