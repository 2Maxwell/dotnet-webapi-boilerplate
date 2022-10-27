using FSH.WebApi.Application.Accounting.Rates;
using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PriceCats;

public class CreatePriceCatRequest : IRequest<int>
{
    public int MandantId { get; set; }
    // public int RateId { get; set; }
    public int CategoryId { get; set; }
    public DateTime DatePrice { get; set; }
    public int Pax { get; set; }
    public decimal RateCurrent { get; set; }
    public decimal RateStart { get; set; }
    public decimal RateAutomatic { get; set; }
    public string? EventDates { get; set; } // Trenner , (Komma)
    public int RateTypeEnumId { get; set; } // Base, Season, Event, Fair
}

public class CreatePriceRateCatRequestValidator : CustomValidator<CreatePriceCatRequest>
{
    public CreatePriceRateCatRequestValidator(IReadRepository<PriceCat> repository, IStringLocalizer<CreatePriceRateCatRequestValidator> localizer)
    {
        RuleFor(x => x.DatePrice)
            .NotEmpty();
        RuleFor(x => x.Pax)
            .GreaterThan(0);
    }
}

public class PriceCatByCatIdSpec : Specification<PriceCat, PriceCatDto>
{
    public PriceCatByCatIdSpec(int categoryId, int mandantId) =>
        Query.Where(x => x.CategoryId == categoryId && (x.MandantId == mandantId || x.MandantId == 0));
}

//public class PriceCatByDateCatRatePaxSpec : Specification<PriceCat, PriceCatDto>
//{
//    public PriceCatByDateCatRatePaxSpec(DateTime date, int categoryId, int rateId, int pax) =>
//        Query.Where(x => x.Date == date && x.CategoryId == categoryId && x.RateId == rateId && x.Pax == pax);
//}

public class CreatePriceCatRequestHandler : IRequestHandler<CreatePriceCatRequest, int>
{
    private readonly IRepositoryWithEvents<PriceCat> _repository;

    public CreatePriceCatRequestHandler(IRepositoryWithEvents<PriceCat> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreatePriceCatRequest request, CancellationToken cancellationToken)
    {
        var PriceCat = new PriceCat(
            request.MandantId,
            request.CategoryId,
            request.DatePrice,
            request.Pax,
            request.RateCurrent,
            request.RateStart,
            request.RateAutomatic,
            request.EventDates,
            request.RateTypeEnumId
            );
        PriceCat.DomainEvents.Add(EntityCreatedEvent.WithEntity(PriceCat));
        await _repository.AddAsync(PriceCat, cancellationToken);

        //RateDto? rateDto = await _rateRepository.GetBySpecAsync(
        //            (ISpecification<Rate, RateDto>)new RateByIdSpec(request.RateId), cancellationToken);

        //CategoryDto? categoryDto = await _categoryRepository.GetBySpecAsync(
        //            (ISpecification<Category, CategoryDto>)new CategoryByIdSpec(request.CategoryId), cancellationToken);

        //int pcr = await _repository.CountAsync(
        //    (ISpecification<PriceCat, PriceCatDto>)new PriceCatByRateIdSpec(request.RateId, request.MandantId), cancellationToken);

        return PriceCat.Id;
    }
}
