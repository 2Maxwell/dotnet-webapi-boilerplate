using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class CreateCategoryRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Kz { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int OrderNumber { get; set; }
    public string? Properties { get; set; }
    public bool VkatRelevant { get; set; }
    public bool VkatDone { get; set; }
    public int NumberOfBeds { get; set; }
    public int NumberOfExtraBeds { get; set; }
    public string? Display { get; set; }
    public string? DisplayShort { get; set; }
    public string? DisplayHighLight { get; set; }
    public bool CategoryIsVirtual { get; set; }
    public int VirtualSourceCategoryId { get; set; }
    public string? VirtualCategoryFormula { get; set; }
    public int VirtualImportCategoryId { get; set; }
    public int VirtualCategoryQuantity { get; set; }
    public int CategoryDefaultQuantity { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
    public string? ConfirmationText { get; set; }

}

public class CreateCategoryRequestValidator : CustomValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator(IReadRepository<Category> repository, IStringLocalizer<CreateCategoryRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100)
        .MustAsync(async (category, name, ct) => await repository.GetBySpecAsync(new CategoryByNameSpec(name, category.MandantId), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["categoryName.alreadyexists"], name));

        RuleFor(x => x.Kz)
        .NotEmpty()
        .MaximumLength(10)
        .MustAsync(async (category, kz, ct) => await repository.GetBySpecAsync(new CategoryByKzSpec(kz, category.MandantId), ct) is null)
                .WithMessage((_, kz) => string.Format(localizer["categoryKz.alreadyexists"], kz));

        RuleFor(x => x.Description)
            .MaximumLength(500);
        RuleFor(x => x.Properties)
            .MaximumLength(500);
        RuleFor(x => x.Display)
            .MaximumLength(500);
        RuleFor(x => x.DisplayShort)
            .MaximumLength(300);
        RuleFor(x => x.DisplayHighLight)
            .MaximumLength(300);
        RuleFor(x => x.VirtualCategoryFormula)
            .MaximumLength(200);
        RuleFor(x => x.ConfirmationText)
            .MaximumLength(500);
        RuleFor(x => x.ChipIcon)
            .MaximumLength(100);
        RuleFor(x => x.ChipText)
            .MaximumLength(50);
    }
}

public class CategoryByKzSpec : Specification<Category, CategoryDto>, ISingleResultSpecification
{
    public CategoryByKzSpec(string kz, int mandantId) =>
        Query.Where(x => x.Kz == kz && (x.MandantId == mandantId));
}

public class CategoryByNameSpec : Specification<Category>, ISingleResultSpecification
{
    public CategoryByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, int>
{
    private readonly IRepositoryWithEvents<Category> _repository;

    public CreateCategoryRequestHandler(IRepositoryWithEvents<Category> repository) => _repository = repository;

    public async Task<int> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new Category(
            request.MandantId,
            request.Kz,
            request.Name,
            request.Description,
            request.OrderNumber,
            request.Properties,
            request.VkatRelevant,
            request.VkatDone,
            request.NumberOfBeds,
            request.NumberOfExtraBeds,
            request.Display,
            request.DisplayShort,
            request.DisplayHighLight,
            request.CategoryIsVirtual,
            request.VirtualSourceCategoryId,
            request.VirtualCategoryFormula,
            request.VirtualImportCategoryId,
            request.VirtualCategoryQuantity,
            request.CategoryDefaultQuantity,
            request.ChipIcon,
            request.ChipText,
            request.ConfirmationText);

        // Add Domain Events to be raised after the commit
        category.DomainEvents.Add(EntityCreatedEvent.WithEntity(category));
        await _repository.AddAsync(category, cancellationToken);

        DateOnly start = new DateOnly(2022, 09, 01); // aktuelles Datum
        DateOnly end = new DateOnly(2022,10,31); // heute bis Anzahl Tage vorlauf in Hoteleinstellungen

        int beds = 1;
        while (beds <= (request.NumberOfBeds + request.NumberOfExtraBeds))
        {

            DateOnly aktDate = start;
            while (aktDate <= end)
            {
                _ = new CreatePriceCatRequest()
                {
                    MandantId = request.MandantId,
                    CategoryId = category.Id,
                    DatePrice = aktDate.ToDateTime(TimeOnly.Parse("12:00 PM")),
                    Pax = beds,
                    RateStart = 9990 + beds,
                    RateAutomatic = 0,
                    RateTypeEnumId = (int)RateTypeEnum.Base,
                };
                aktDate = aktDate.AddDays(1);
            }

            beds++;
        }

        return category.Id;
    }
}
