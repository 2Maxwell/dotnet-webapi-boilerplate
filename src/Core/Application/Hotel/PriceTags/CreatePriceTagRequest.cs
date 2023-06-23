using FSH.WebApi.Application.Hotel.Reservations;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.PriceTags;
public class CreatePriceTagRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public int ReservationId { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public decimal AverageRate { get; set; }
    public decimal? UserRate { get; set; }
    public int RateSelected { get; set; } // 1 = Daily, 2 = AverageRate, 3 = UserRate
    public int CategoryId { get; set; }
    public List<PriceTagDetail> PriceTagDetails { get; set; }
}

public class CreatePriceTagRequestValidator : CustomValidator<CreatePriceTagRequest>
{
    public CreatePriceTagRequestValidator(IReadRepository<PriceTag> repository, IStringLocalizer<CreatePriceTagRequestValidator> localizer)
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

public class CreatePriceTagRequestHandler : IRequestHandler<CreatePriceTagRequest, int>
{
    private readonly IRepository<PriceTag> _repository;
    private readonly IRepository<PriceTagDetail> _repositoryPriceTagDetail;

    public CreatePriceTagRequestHandler(IRepository<PriceTag> repository, IRepository<PriceTagDetail> repositoryPriceTagDetail)
    {
        _repository = repository;
        _repositoryPriceTagDetail = repositoryPriceTagDetail;
    }

    public async Task<int> Handle(CreatePriceTagRequest request, CancellationToken cancellationToken)
    {
        var priceTag = new PriceTag(
                                    request.MandantId,
                                    request.ReservationId,
                                    request.Arrival,
                                    request.Departure,
                                    request.AverageRate,
                                    request.UserRate,
                                    request.RateSelected,
                                    request.CategoryId);
        priceTag.DomainEvents.Add(EntityCreatedEvent.WithEntity(priceTag));
        await _repository.AddAsync(priceTag, cancellationToken);

        foreach (PriceTagDetail tagDetail in request.PriceTagDetails)
        {
            PriceTagDetail ptd = new PriceTagDetail(
                                        priceTag.Id,
                                        priceTag.CategoryId,
                                        tagDetail.RateId,
                                        tagDetail.DatePrice,
                                        tagDetail.PaxAmount,
                                        tagDetail.RateCurrent,
                                        tagDetail.RateStart,
                                        tagDetail.RateAutomatic,
                                        Convert.ToDecimal(priceTag.UserRate),
                                        priceTag.AverageRate,
                                        tagDetail.EventDates,
                                        tagDetail.RateTypeEnumId,
                                        tagDetail.NoShow,
                                        tagDetail.NoShowPercentage);
            await _repositoryPriceTagDetail.AddAsync(ptd, cancellationToken);
        }

        return priceTag.Id;
    }
}
