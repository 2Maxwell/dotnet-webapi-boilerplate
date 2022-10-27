using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PriceCats;

public class UpdatePriceCatRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public decimal RateCurrent { get; set; }
    public decimal RateStart { get; set; }
    public decimal RateAutomatic { get; set; }
    public string? EventDates { get; set; } // Trenner , (Komma)
    public int RateTypeEnumId { get; set; } // Base, Season, Event, Fair
}

public class UpdatePriceCatRequestValidator : CustomValidator<UpdatePriceCatRequest>
{
    public UpdatePriceCatRequestValidator(IReadRepository<PriceCat> repository, IStringLocalizer<UpdatePriceCatRequestValidator> localizer)
    {
        RuleFor(x => x.RateStart)
            .GreaterThan(0);
    }
}

public class UpdatePriceCatRequestHandler : IRequestHandler<UpdatePriceCatRequest, int>
{
    private readonly IRepositoryWithEvents<PriceCat> _repository;
    private readonly IStringLocalizer<UpdatePriceCatRequestHandler> _localizer;

    public UpdatePriceCatRequestHandler(IRepositoryWithEvents<PriceCat> repository, IStringLocalizer<UpdatePriceCatRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdatePriceCatRequest request, CancellationToken cancellationToken)
    {
        var priceCat = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = priceCat ?? throw new NotFoundException(string.Format(_localizer["priceCatRate.notfound"], request.Id));
        priceCat.Update(
            request.RateCurrent,
            request.RateCurrent - request.RateAutomatic,
            request.RateAutomatic,
            request.EventDates,     // TODO EventDates könnten überschrieben werden bei neuen Updates
            request.RateTypeEnumId
            );

        priceCat.DomainEvents.Add(EntityUpdatedEvent.WithEntity(priceCat));
        await _repository.UpdateAsync(priceCat, cancellationToken);

        return request.Id;
    }
}
