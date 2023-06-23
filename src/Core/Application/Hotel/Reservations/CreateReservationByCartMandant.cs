using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Application.Hotel.VCats;
using FSH.WebApi.Application.ShopMandant.CartMandants;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;
using System.Text.Json;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class CreateReservationByCartMandantRequest : IRequest<int>
{
    public CartMandantDto cart { get; set; }
    public CartItemMandantDto item { get; set; }
}

public class CreateReservationByCartMandantRequestValidator : CustomValidator<CreateReservationByCartMandantRequest>
{
    public CreateReservationByCartMandantRequestValidator(IReadRepository<Reservation> repository, IStringLocalizer<CreateReservationByCartMandantRequestValidator> localizer)
    {
        RuleFor(x => x.cart.MandantId)
            .NotEmpty();
        RuleFor(x => x.cart.PersonId)
            .GreaterThan(0)
            .WithMessage((_, PersonId) => localizer["Booker must be choosen or set!", PersonId]);
        RuleFor(x => x.item.Start)
            .LessThanOrEqualTo(x => x.item.End);
        RuleFor(x => x.item.End)
            .GreaterThanOrEqualTo(x => x.item.Start);
        RuleFor(x => x.cart!.MatchCode)
            .MaximumLength(25)
            .WithMessage((_, MatchCode) => localizer["Validation MatchCode.", MatchCode]);
    }
}

public class CreateReservationByCartMandantRequestHandler : IRequestHandler<CreateReservationByCartMandantRequest, int>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IRepository<PriceTag> _repositoryPriceTag;
    private readonly IRepository<PriceTagDetail> _repositoryPriceTagDetail;
    private readonly IRepository<PackageExtend> _repositoryPackageExtend;
    private readonly IRepository<VCat> _repositoryVCat;
    private readonly IRepository<Category> _repositoryCategory;

    public CreateReservationByCartMandantRequestHandler(IRepository<Reservation> repository, IRepository<PriceTag> repositoryPriceTag, IRepository<PriceTagDetail> repositoryPriceTagDetail, IRepository<PackageExtend> repositoryPackageExtend, IRepository<VCat> repositoryVCat, IRepository<Category> repositoryCategory)
    {
        _repository = repository;
        _repositoryPriceTag = repositoryPriceTag;
        _repositoryPriceTagDetail = repositoryPriceTagDetail;
        _repositoryPackageExtend = repositoryPackageExtend;
        _repositoryVCat = repositoryVCat;
        _repositoryCategory = repositoryCategory;
    }

    public async Task<int> Handle(CreateReservationByCartMandantRequest request, CancellationToken cancellationToken)
    {
        var reservation = new Reservation(
                                request.cart!.MandantId,
                                "P",
                                request.cart.PersonId,
                                request.cart.BookerIsGuest ? request.cart.PersonId : null,
                                request.cart.CompanyId > 0 ? request.cart.CompanyId : null,
                                request.cart.CompanyContactId > 0 ? request.cart.CompanyContactId : null,
                                request.cart.TravelAgentId > 0 ? request.cart.TravelAgentId : null,
                                request.cart.TravelAgentContactId > 0 ? request.cart.TravelAgentContactId : null,
                                request.item!.PersonList != null ? JsonSerializer.Serialize(request.item.PersonList) : string.Empty,
                                // TODO ArrivalTime und DepartureTime auf MandantSettings stellen.
                                Convert.ToDateTime(request.item.Start).AddHours(17).AddMinutes(30),
                                Convert.ToDateTime(request.item.End).AddHours(8),
                                request.item.CategoryId,
                                request.item.Amount,
                                0,
                                string.Empty,
                                false,
                                request.item.RateId,
                                request.item.RatePackages,
                                request.item.PriceTotal,
                                request.cart.BookingPolicyId,
                                request.cart.CancellationPolicyId,
                                false, //request.IsGroupMaster, // TODO Prozedur erstellen
                                0, //request.GroupMasterId, // TODO aus MandantDetails ziehen
                                string.Empty, //request.Transfer,   // TODO aus MandantDetails ziehen
                                request.cart.MatchCode, // TODO MatchCode erstellen
                                null, // request.cart.OptionDate, // TODO
                                0, //request.OptionFollowUp, // TODO
                                null,
                                request.item.Pax != null ? JsonSerializer.Serialize(request.item.Pax) : string.Empty,
                                request.cart.CartId!.Value, // Guid.NewGuid(), //request.CartId, // TODO
                                request.cart.Confirmations, //request.CartItemId) // TODO;
                                request.item.Wishes);

        // Packages oder PackagesExtended
        // PackageExtendedBookingLines
        // Remarks
        reservation.DomainEvents.Add(EntityCreatedEvent.WithEntity(reservation));
        await _repository.AddAsync(reservation, cancellationToken);

        Category category = await _repositoryCategory.GetByIdAsync(reservation.CategoryId, cancellationToken);


        #region VKat Calculation RollInReservation

        int nights = Convert.ToInt32((reservation.Departure - reservation.Arrival).TotalDays);
        for (int i = 0; i < nights; i++)
        {
            var spec = new VCatByMandantDateCategorySpec(reservation.MandantId, reservation.Arrival.AddDays(i), reservation.CategoryId);
            var vCat = await _repositoryVCat.GetBySpecAsync(spec, cancellationToken);

            int bedsAdult = request.item.Pax.Adult;
            int extraBeds = 0;
            if (bedsAdult <= category.NumberOfBeds)
            {
                bedsAdult = bedsAdult * (int)reservation.RoomAmount;
            }
            else
            {
                bedsAdult = category.NumberOfBeds * (int)reservation.RoomAmount;
                extraBeds = (bedsAdult - category.NumberOfBeds) * (int)reservation.RoomAmount;
            }

            extraBeds += request.item.Pax.Children.Count(x => x.ExtraBed) * (int)reservation.RoomAmount;

            vCat.RollInByReservation(
                (int)reservation.RoomAmount,
                    i == 0 ? 0 : (int)reservation.RoomAmount,
                i == 0 ? (int)reservation.RoomAmount : 0,
                bedsAdult,
                extraBeds,
                request.item.Pax.Adult * (int)reservation.RoomAmount,
                request.item.Pax.Children.Count() * (int)reservation.RoomAmount,
                0,
                0);
            await _repositoryVCat.UpdateAsync(vCat, cancellationToken);
        }

        var specDeparture = new VCatByMandantDateCategorySpec(reservation.MandantId, reservation.Departure, reservation.CategoryId);
        var vCatDeparture = await _repositoryVCat.GetBySpecAsync(specDeparture, cancellationToken);
        vCatDeparture.RollInByReservation(
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                (int)reservation.RoomAmount,
                0);
        await _repositoryVCat.UpdateAsync(vCatDeparture, cancellationToken);

        #endregion

        decimal average = request.item.PriceCats.Sum(x => x.RateCurrent) / request.item.PriceCats.Count;

        PriceTag priceTag = new(
            request.cart.MandantId,
            reservation.Id,
            reservation.Arrival,
            reservation.Departure,
            average,
            null,
            1,
            reservation.CategoryId);
        priceTag.DomainEvents.Add(EntityCreatedEvent.WithEntity(priceTag));
        await _repositoryPriceTag.AddAsync(priceTag, cancellationToken);

        foreach (PriceCatDto priceCat in request.item.PriceCats!)
        {
            PriceTagDetail ptd = new(
                priceTag.Id,
                reservation.CategoryId,
                reservation.RateId,
                priceCat.DatePrice,
                priceCat.Pax,
                priceCat.RateCurrent,
                priceCat.RateStart,
                priceCat.RateAutomatic,
                null,
                average,
                null,
                priceCat.RateTypeEnumId,
                false,
                null);

            ptd.DomainEvents.Add(EntityCreatedEvent.WithEntity(ptd));
            await _repositoryPriceTagDetail.AddAsync(ptd, cancellationToken);
        }

        if (request.item.PackageExtendedList != null && request.item.PackageExtendedList.Count > 0)
        {
            foreach (PackageExtendDto peDto in request.item.PackageExtendedList)
            {
                PackageExtend packageExtend = new(
                    request.cart.MandantId,
                    peDto.PackageDto.Id,
                    peDto.ImagePath,
                    peDto.Amount,
                    peDto.Price,
                    peDto.Appointment,
                    peDto.AppointmentSource,
                    peDto.AppointmentSourceId,
                    PackageExtendedStateEnum.pending,
                    PackageExtendSourceEnum.HotelReservation,
                    reservation.Id,
                    peDto.Duration);
                packageExtend.DomainEvents.Add(EntityCreatedEvent.WithEntity(packageExtend));
                await _repositoryPackageExtend.AddAsync(packageExtend, cancellationToken);
            }
        }

        return reservation.Id;
    }
}