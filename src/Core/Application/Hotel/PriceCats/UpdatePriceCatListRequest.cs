using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PriceCats;
public class UpdatePriceCatsListRequest : IRequest<int>
{
    public List<PriceCatDto> priceCatDtos { get; set; }
}

public class UpdatePriceCatsListRequestHandler : IRequestHandler<UpdatePriceCatsListRequest, int>
{
    private readonly IRepositoryWithEvents<PriceCat> _repository;
    private readonly IStringLocalizer<UpdatePriceCatsListRequestHandler> _localizer;

    public UpdatePriceCatsListRequestHandler(IRepositoryWithEvents<PriceCat> repository, IStringLocalizer<UpdatePriceCatsListRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdatePriceCatsListRequest request, CancellationToken cancellationToken)
    {
        int counter = 0;

        foreach (PriceCatDto item in request.priceCatDtos)
        {
            var priceCat = await _repository.GetByIdAsync(item.Id, cancellationToken);
            _ = priceCat ?? throw new NotFoundException(string.Format(_localizer["priceCatRate.notfound"], item.Id));
            priceCat.Update(
                item.RateCurrent,
                item.RateCurrent - item.RateAutomatic,
                item.RateAutomatic,
                item.EventDates,     // TODO EventDates könnten überschrieben werden bei neuen Updates
                item.RateTypeEnumId
            );

            priceCat.DomainEvents.Add(EntityUpdatedEvent.WithEntity(priceCat));
            await _repository.UpdateAsync(priceCat, cancellationToken);
            counter++;
        }

        return counter;
    }
}