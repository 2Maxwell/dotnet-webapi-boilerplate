using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Categorys;

public class UpdateCategoryRequest : IRequest<int>
{
    public int Id { get; set; }
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

public class UpdateCategoryRequestValidator : CustomValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator(IReadRepository<Category> repository, IStringLocalizer<UpdateCategoryRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100)
        .MustAsync(async (category, name, ct) =>
                await repository.GetBySpecAsync(new CategoryByNameSpec(name, category.MandantId), ct)
                is not Category existingCategory || existingCategory.Id == category.Id)
                .WithMessage((_, name) => string.Format(localizer["categoryName.alreadyexists"], name));

        RuleFor(x => x.Kz)
        .NotEmpty()
        .MaximumLength(10)
        .MustAsync(async (category, kz, ct) =>
                await repository.GetBySpecAsync(new CategoryByKzSpec(kz, category.MandantId), ct)
                is not Category existingCategory || existingCategory.Id == category.Id)
                .WithMessage((_, kz) => string.Format(localizer["categoryKz.alreadyexists"], kz));

        RuleFor(x => x.Description)
            .MaximumLength(500);
        RuleFor(x => x.Properties)
            .MaximumLength(500);
        RuleFor(x => x.Description)
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

public class UpdateCategoryRequestHandler : IRequestHandler<UpdateCategoryRequest, int>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Category> _repository;
    private readonly IStringLocalizer<UpdateCategoryRequestHandler> _localizer;
    private readonly IRepositoryWithEvents<PriceCat> _priceCatRateRepository;

    public UpdateCategoryRequestHandler(IRepositoryWithEvents<Category> repository, IRepositoryWithEvents<PriceCat> priceCatRateRepository, IStringLocalizer<UpdateCategoryRequestHandler> localizer) =>
        (_repository, _priceCatRateRepository, _localizer) = (repository, priceCatRateRepository, localizer);

    public async Task<int> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = category ?? throw new NotFoundException(string.Format(_localizer["category.notfound"], request.Id));

        category.Update(
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

        await _repository.UpdateAsync(category, cancellationToken);

        int existingRecords = await _priceCatRateRepository.CountAsync(
    (ISpecification<PriceCat, PriceCatDto>)new PriceCatByCatIdSpec(request.Id, request.MandantId), cancellationToken);

        if (existingRecords == 0)
        {
            DateOnly start = new DateOnly(2022, 09, 01); // TODO aktuelles Datum
            DateOnly end = new DateOnly(2022, 10, 31); // TODO heute bis Anzahl Tage vorlauf in Hoteleinstellungen
            List<CreatePriceCatRequest> priceCatList = new List<CreatePriceCatRequest>();

            int beds = 1;
            while (beds <= (request.NumberOfBeds + request.NumberOfExtraBeds))
            {

                DateOnly aktDate = start;
                while (aktDate <= end)
                {
                    CreatePriceCatRequest createPriceCatRequest = new CreatePriceCatRequest()
                    {
                        MandantId = request.MandantId,
                        CategoryId = category.Id,
                        DatePrice = aktDate.ToDateTime(TimeOnly.Parse("12:00 PM")),
                        Pax = beds,
                        RateStart = 9990 + beds,
                        RateAutomatic = 0,
                        RateTypeEnumId = (int)RateTypeEnum.Base,
                    };
                    priceCatList.Add(createPriceCatRequest);
                    aktDate = aktDate.AddDays(1);
                }

                beds++;
            }

            CreatePriceCatRequestHandler createPriceCatRequestHandler = new CreatePriceCatRequestHandler(_priceCatRateRepository);
            foreach (CreatePriceCatRequest item in priceCatList)
            {
                _ = await createPriceCatRequestHandler.Handle(item, cancellationToken);
            }

        }

        return request.Id;
    }
}