using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Accounting.Rates;

public class UpdateRateRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? DisplayShort { get; set; }
    public string? Display { get; set; }
    public int BookingPolicyId { get; set; }
    public int CancellationPolicyId { get; set; }
    public string? Packages { get; set; } // Value ist Packages.Kz mit , als Trenner
    public IEnumerable<string> IEPackages
    {
        get
        {
            IEnumerable<string> value = Packages.Split(',', StringSplitOptions.TrimEntries).AsEnumerable();
            return value;
        }
    }

    public string? Categorys { get; set; } // Value ist Category mit , als Trenner
    public IEnumerable<string> IECategorys
    {
        get
        {
            // string[] getrennt = Categorys.Split(',');
            IEnumerable<string> value = Categorys.Split(',', StringSplitOptions.TrimEntries).AsEnumerable();
            return value;
        }
    }

    public bool RuleFlex { get; set; }
    public int RateTypeEnumId { get; set; } // Base, Season, Event, Fair
    public int RateScopeEnumId { get; set; } // Public, Private
}

public class UpdateRateRequestValidator : CustomValidator<UpdateRateRequest>
{
    public UpdateRateRequestValidator(IReadRepository<Rate> repository, IStringLocalizer<UpdateRateRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (rate, name, ct) =>
            await repository.GetBySpecAsync(new RateByNameSpec(name, rate.MandantId), ct)
            is not Rate existingRate || existingRate.MandantId == rate.MandantId)
            .WithMessage((_, name) => string.Format(localizer["rateName.alreadyexists"], name));

        RuleFor(x => x.Kz)
            .NotEmpty()
            .MaximumLength(10)
            .MustAsync(async (rate, kz, ct) =>
            await repository.GetBySpecAsync(new RateByKzSpec(kz, rate.MandantId), ct)
            is not Rate existingRate || existingRate.MandantId == rate.MandantId)
            .WithMessage((_, kz) => string.Format(localizer["rateKz.alreadyexists"], kz));

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.DisplayShort)
            .NotEmpty()
           .MaximumLength(150);
        RuleFor(x => x.Display)
            .NotEmpty()
            .MaximumLength(500);
        RuleFor(x => x.Packages)
            .MaximumLength(200);
        RuleFor(x => x.Categorys)
            .MaximumLength(200);
    }
}

public class UpdateRateRequestHandler : IRequestHandler<UpdateRateRequest, int>
{
    private readonly IRepositoryWithEvents<Rate> _repository;
    private readonly IReadRepository<Category> _categoryRepository;
    private readonly IStringLocalizer<UpdateRateRequestHandler> _localizer;
    private readonly IRepositoryWithEvents<PriceCat> _priceCatRateRepository;

    public UpdateRateRequestHandler(IRepositoryWithEvents<Rate> repository, IReadRepository<Category> categoryRepository, IRepositoryWithEvents<PriceCat> priceCatRateRepository, IStringLocalizer<UpdateRateRequestHandler> localizer)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _priceCatRateRepository = priceCatRateRepository;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdateRateRequest request, CancellationToken cancellationToken)
    {
        if (request.Packages.StartsWith(',')) { request.Packages = request.Packages.Remove(0, 1); }
        var rate = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = rate ?? throw new NotFoundException(string.Format(_localizer["rate.notfound"], request.Id));
        rate.Update(
            request.Name,
            request.Kz,
            request.Description,
            request.DisplayShort,
            request.Display,
            request.BookingPolicyId,
            request.CancellationPolicyId,
            request.Packages,
            request.Categorys,
            request.RuleFlex,
            request.RateTypeEnumId,
            request.RateScopeEnumId
            );

        rate.DomainEvents.Add(EntityUpdatedEvent.WithEntity(rate));
        await _repository.UpdateAsync(rate, cancellationToken);

        return request.Id;


        // vorhandene PriceCat auf vollständigkeit prüfen

        //DateOnly start = new DateOnly(2022, 09, 01); // aktuelles Datum
        //DateOnly end = new DateOnly(2022, 10, 31); // heute bis Anzahl Tage vorlauf in Hoteleinstellungen

        //string[] cats = request.Categorys.Split(',').Select(x => x.Trim()).ToArray();
        //List<CategoryDto> categoryList = new List<CategoryDto>();

        //// ermitteln wieviele Datensätze sich aus der Rate ergeben. (counter)

        //foreach (string cat in cats)
        //{
        //    CategoryDto? catDto = await _categoryRepository.GetBySpecAsync((ISpecification<Category, CategoryDto>)new CategoryByKzSpec(cat, request.MandantId), cancellationToken);
        //    categoryList.Add(catDto!);
        //}

        //int counter = 0;

        //foreach (CategoryDto categoryDto in categoryList)
        //{
        //    int beds = 1;
        //    while (beds <= (categoryDto.NumberOfBeds + categoryDto.NumberOfExtraBeds))
        //    {
        //        DateOnly aktDate = start;
        //        while (aktDate <= end)
        //        {
        //            counter++;
        //            aktDate = aktDate.AddDays(1);
        //        }

        //        beds++;
        //    }
        //}

        //Debug.Print("Probelauf gezählt: " + counter);

        //// in der Datenbank nachzählen wieviele Datensätze vorhanden sind

        //int existingRecord = await _priceCatRateRepository.CountAsync(
        //    (ISpecification<PriceCat, PriceCatDto>)new PriceCatByRateIdSpec(request.Id, request.MandantId), cancellationToken);

        //// wenn ein Unterschied besteht (counter größer existingRecord) - alle Datensätze erzeugen und auf nicht vorhandene in DB prüfen und erzeugen.

        //if (existingRecord < counter)
        //{
        //    List<CreatePriceCatRequest> priceCatRateList = new List<CreatePriceCatRequest>();
        //    foreach (CategoryDto categoryDto in categoryList)
        //    {
        //        int beds = 1;
        //        while (beds <= (categoryDto.NumberOfBeds + categoryDto.NumberOfExtraBeds))
        //        {
        //            DateOnly aktDate = start;
        //            while (aktDate <= end)
        //            {
        //                CreatePriceCatRequest createPriceCatRequest = new CreatePriceCatRequest()
        //                {
        //                    MandantId = request.MandantId,
        //                    // RateId = rate.Id,
        //                    CategoryId = categoryDto.Id,
        //                    Date = aktDate.ToDateTime(TimeOnly.Parse("12:00 PM")),
        //                    Pax = beds,
        //                    // RateCurrent = 9990 + beds,
        //                    RateStart = 9990,
        //                    RateAutomatic = 0,
        //                    RateTypeEnumId = request.RateTypeEnumId
        //                };
        //                priceCatRateList.Add(createPriceCatRequest);
        //                aktDate = aktDate.AddDays(1);
        //            }

        //            beds++;
        //        }
        //    }

        //    CreatePriceCatRequestHandler createPriceCat = new CreatePriceCatRequestHandler(_priceCatRateRepository);

        //    foreach (CreatePriceCatRequest item in priceCatRateList)
        //    {
        //        PriceCatDto? priceCatRate = await _priceCatRateRepository.GetBySpecAsync((ISpecification<PriceCat, PriceCatDto>)
        //            new PriceCatByDateCatRatePaxSpec(item.Date, item.CategoryId, item.RateId, item.Pax));

        //        if (priceCatRate == null)
        //        {
        //            _ = await createPriceCat.Handle(item, cancellationToken);
        //        }
        //    }
        //}

        //// TODO Löschen zum Abgleich organisieren
        //// wenn ein Unterschied besteht (Counter kleiner existingRecord) alle Datensätze aus DB laden und mit priceCatRateList vergleichen
        //// wenn in DB vorhanden aber nicht in list dann löschen

    }
}
