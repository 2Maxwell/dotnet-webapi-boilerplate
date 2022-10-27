using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.PriceCats;
public class UpdatePriceCatSinglePriceRequest : IRequest<int>
{
    public int Id { get; set; }
    public decimal RateCurrent { get; set; }
}

public class UpdatePriceCatSinglePriceRequestValidator : CustomValidator<UpdatePriceCatSinglePriceRequest>
{
    public UpdatePriceCatSinglePriceRequestValidator(IReadRepository<PriceCat> repository, IStringLocalizer<UpdatePriceCatSinglePriceRequestValidator> localizer)
    {
        RuleFor(x => x.RateCurrent)
            .GreaterThan(0);
    }
}

public class UpdatePriceCatSinglePriceRequestHandler : IRequestHandler<UpdatePriceCatSinglePriceRequest, int>
{
    private readonly IRepositoryWithEvents<PriceCat> _repository;
    private readonly IStringLocalizer<UpdatePriceCatSinglePriceRequestHandler> _localizer;

    public UpdatePriceCatSinglePriceRequestHandler(IRepositoryWithEvents<PriceCat> repository, IStringLocalizer<UpdatePriceCatSinglePriceRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdatePriceCatSinglePriceRequest request, CancellationToken cancellationToken)
    {
        var priceCat = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = priceCat ?? throw new NotFoundException(string.Format(_localizer["priceCatRate.notfound"], request.Id));
        priceCat.UpdateSinglePrice(
            request.RateCurrent,
            request.RateCurrent - priceCat.RateAutomatic
            );

        priceCat.DomainEvents.Add(EntityUpdatedEvent.WithEntity(priceCat));
        await _repository.UpdateAsync(priceCat, cancellationToken);

        return request.Id;
    }
}
