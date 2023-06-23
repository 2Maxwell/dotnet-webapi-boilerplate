using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PriceTags;
public class UpdatePriceTagRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public int ReservationId { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public decimal AverageRate { get; set; }
    public decimal? UserRate { get; set; }
    public int RateSelected { get; set; } // 1 = Daily, 2 = AverageRate, 3 = UserRate
    public int CategoryId { get; set; }
    public List<PriceTagDetail>? PriceTagDetails { get; set; }
}

public class UpdatePriceTagRequestValidator : CustomValidator<UpdatePriceTagRequest>
{
    public UpdatePriceTagRequestValidator(IReadRepository<PriceTag> repository, IStringLocalizer<UpdatePriceTagRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.ReservationId)
            .NotEmpty();
        RuleFor(x => x.Arrival)
            .LessThanOrEqualTo(x => x.Departure);
        RuleFor(x => x.Departure)
            .GreaterThanOrEqualTo(x => x.Arrival);
        RuleFor(x => x.AverageRate)
            .NotEmpty();
        RuleFor(x => x.RateSelected)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.CategoryId)
            .NotEmpty();
        RuleFor(x => x.PriceTagDetails)
            .NotEmpty();
    }
}

public class UpdatePriceTagRequestHandler : IRequestHandler<UpdatePriceTagRequest, int>
{
    private readonly IRepository<PriceTag> _repository;
    private readonly IRepository<PriceTagDetail> _repositoryPriceTagDetail;
    private readonly IStringLocalizer<UpdatePriceTagRequestHandler> _localizer;

    public UpdatePriceTagRequestHandler(IRepository<PriceTag> repository, IRepository<PriceTagDetail> repositoryPriceTagDetail, IStringLocalizer<UpdatePriceTagRequestHandler> localizer)
    {
        _repository = repository;
        _repositoryPriceTagDetail = repositoryPriceTagDetail;
        _localizer = localizer;
    }

    public async Task<int> Handle(UpdatePriceTagRequest request, CancellationToken cancellationToken)
    {
        var priceTag = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = priceTag ?? throw new NotFoundException(string.Format(_localizer["priceTag not found"], request.Id));
        priceTag.Update(
                                    request.ReservationId,
                                    request.Arrival,
                                    request.Departure,
                                    request.AverageRate,
                                    request.UserRate,
                                    request.RateSelected,
                                    request.CategoryId);
        priceTag.DomainEvents.Add(EntityUpdatedEvent.WithEntity(priceTag));
        await _repository.AddAsync(priceTag, cancellationToken);

        foreach (PriceTagDetail tagDetail in request.PriceTagDetails!)
        {
            var priceTagDetail = await _repositoryPriceTagDetail.GetByIdAsync(tagDetail.Id, cancellationToken);
            _ = priceTagDetail ?? throw new NotFoundException(string.Format(_localizer["priceTagDetail not found"], request.Id));

            priceTagDetail.Update(
                            Convert.ToDecimal(priceTag.UserRate),
                            Convert.ToDecimal(priceTag.AverageRate),
                            tagDetail.NoShow,
                            tagDetail.NoShowPercentage);
            priceTagDetail.DomainEvents.Add(EntityCreatedEvent.WithEntity(priceTagDetail));
            await _repositoryPriceTagDetail.AddAsync(priceTagDetail, cancellationToken);
            return priceTagDetail.Id;
        }

        return priceTag.Id;
    }
}
