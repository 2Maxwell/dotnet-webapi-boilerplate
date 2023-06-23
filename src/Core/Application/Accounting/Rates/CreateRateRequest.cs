using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Accounting.Rates;

public class CreateRateRequest : IRequest<int>
{
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
    public string? DisplayHighLight { get; set; }
    public string? ConfirmationText { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
}

public class CreateRateRequestValidator : CustomValidator<CreateRateRequest>
{
    public CreateRateRequestValidator(IReadRepository<Rate> repository, IStringLocalizer<CreateRateRequestValidator> localizer)
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
            await repository.GetBySpecAsync(new RateByKzSpec(kz, rate.MandantId), ct) is null)
            .WithMessage((_, kz) => string.Format(localizer["rateKz.alreadyexists"], kz));

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.Display)
            .MaximumLength(500);
        RuleFor(x => x.DisplayShort)
            .MaximumLength(300);
        RuleFor(x => x.DisplayHighLight)
            .MaximumLength(300);
        RuleFor(x => x.ConfirmationText)
            .MaximumLength(500);
        RuleFor(x => x.ChipIcon)
            .MaximumLength(100);
        RuleFor(x => x.ChipText)
            .MaximumLength(50);
        RuleFor(x => x.Packages)
            .MaximumLength(200);
        RuleFor(x => x.Categorys)
            .MaximumLength(200);
    }
}

public class RateByNameSpec : Specification<Rate>, ISingleResultSpecification
{
    public RateByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class RateByKzSpec : Specification<Rate>, ISingleResultSpecification
{
    public RateByKzSpec(string kz, int mandantId) =>
        Query.Where(x => x.Kz == kz && (x.MandantId == mandantId || x.MandantId == 0));
}


public class CreateRateRequestHandler : IRequestHandler<CreateRateRequest, int>
{
    private readonly IRepository<Rate> _repository;
    private readonly IReadRepository<Category> _categoryRepository;

    public CreateRateRequestHandler(IRepository<Rate> repository, IReadRepository<Category> categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<int> Handle(CreateRateRequest request, CancellationToken cancellationToken)
    {
        if (request.Packages.StartsWith(',')) { request.Packages = request.Packages.Remove(0, 1); }
        var rate = new Rate(
            request.MandantId,
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
            request.RateScopeEnumId,
            request.DisplayHighLight,
            request.ConfirmationText,
            request.ChipIcon,
            request.ChipText);

        if(!rate.Packages.Contains("logis")) rate.Packages = "logis, " + rate.Packages;

        rate.DomainEvents.Add(EntityCreatedEvent.WithEntity(rate));
        await _repository.AddAsync(rate, cancellationToken);

        // CreatePriceCatRate first Init

        //DateOnly start = new DateOnly(2022, 09, 01); // aktuelles Datum
        //DateOnly end = new DateOnly(2022,10,31); // heute bis Anzahl Tage vorlauf in Hoteleinstellungen

        //string[] cats = request.Categorys.Split(',');
        //List<CategoryDto> categoryList = new List<CategoryDto>();
        //foreach (string cat in cats)
        //{
        //    CategoryDto? catDto = await _categoryRepository.GetBySpecAsync((ISpecification<Category, CategoryDto>)new CategoryByKzSpec(cat, request.MandantId), cancellationToken);
        //    categoryList.Add(catDto!);
        //}

        //foreach(CategoryDto categoryDto in categoryList)
        //{
        //    int beds = 1;
        //    while (beds <= (categoryDto.NumberOfBeds + categoryDto.NumberOfExtraBeds))
        //    {
        //        DateOnly aktDate = start;
        //        while (aktDate <= end)
        //        {
        //            _ = new CreatePriceCatRateRequest()
        //            {
        //                MandantId = request.MandantId,
        //                RateId = rate.Id,
        //                CategoryId = categoryDto.Id,
        //                Date = aktDate.ToDateTime(TimeOnly.Parse("12:00 PM")),
        //                Pax = beds,
        //                RateCurrent = 9990 + beds,
        //                RateStart = 9990,
        //                RateAutomatic = 0,
        //                RateTypeEnumId = request.RateTypeEnumId,
        //            };
        //            aktDate = aktDate.AddDays(1);
        //        }

        //        beds++;
        //    }
        //}

        return rate.Id;
    }
}