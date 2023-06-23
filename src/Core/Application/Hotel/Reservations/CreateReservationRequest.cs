using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Application.Hotel.PriceTags;
using FSH.WebApi.Application.Hotel.VCats;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;
using Mapster;
using System.Text.Json;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class CreateReservationRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? ResKz { get; set; } // A = Offer, P = Pending, R = Reservation, C = CheckedIn, O = CheckedOut, I = CartItem
    public int BookerId { get; set; }
    public int GuestId { get; set; }
    public int CompanyId { get; set; }
    public int CompanyContactId { get; set; }
    public int TravelAgentId { get; set; }
    public int TravelAgentContactId { get; set; }
    public string? Persons { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public int CategoryId { get; set; }
    public int RoomAmount { get; set; }
    public int RoomNumberId { get; set; }
    public string? RoomNumber { get; set; }
    public bool RoomFixed { get; set; }
    public int RateId { get; set; }
    public string? RatePackages { get; set; }
    public decimal LogisTotal { get; set; }
    public int BookingPolicyId { get; set; }
    public int CancellationPolicyId { get; set; }
    public bool IsGroupMaster { get; set; }
    public int GroupMasterId { get; set; }
    public string? Transfer { get; set; }
    public string? MatchCode { get; set; }
    public DateTime? OptionDate { get; set; }
    public int OptionFollowUp { get; set; } // Aktion bei erreichen des OptionDate (Delete, Request, ...)
    public string? CRSNumber { get; set; }
    public string? PaxString { get; set; } // um Pax zu erzeugen
    public Guid? CartId { get; set; }
    //public Guid CartItemId { get; set; }
    public string? Confirmations { get; set; }
    public string? Wishes { get; set; }
    public List<PackageExtendDto>? PackageExtendList { get; set; }
    public PriceTagDto PriceTag { get; set; }

}

public class CreateReservationRequestValidator : CustomValidator<CreateReservationRequest>
{
    public CreateReservationRequestValidator(IReadRepository<Reservation> repository, IStringLocalizer<CreateReservationRequestValidator> localizer)
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.ResKz)
            .NotEmpty()
            .MaximumLength(1);
        RuleFor(x => x.Arrival)
            .LessThanOrEqualTo(x => x.Departure);
        RuleFor(x => x.Departure)
            .GreaterThanOrEqualTo(x => x.Arrival);
        RuleFor(x => x.RoomNumber)
            .MaximumLength(25);
        RuleFor(x => x.Transfer)
            .MaximumLength(50);
        RuleFor(x => x.MatchCode)
            .MaximumLength(25);
        RuleFor(x => x.CRSNumber)
            .MaximumLength(25);
        RuleFor(x => x.PaxString)
            .MaximumLength(100);
    }
}

public class CreateReservationRequestHandler : IRequestHandler<CreateReservationRequest, int>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IRepository<PriceTag> _repositoryPriceTag;
    private readonly IRepository<PriceTagDetail> _repositoryPriceTagDetail;
    private readonly IRepository<PackageExtend> _repositoryPackageExtend;
    private readonly IRepository<VCat> _repositoryVCat;
    private readonly IRepository<Category> _repositoryCategory;

    public CreateReservationRequestHandler(IRepository<Reservation> repository, IRepository<PriceTag> repositoryPriceTag, IRepository<PriceTagDetail> repositoryPriceTagDetail, IRepository<PackageExtend> repositoryPackageExtend, IRepository<VCat> repositoryVCat, IRepository<Category> repositoryCategory)
    {
        _repository = repository;
        _repositoryPriceTag = repositoryPriceTag;
        _repositoryPriceTagDetail = repositoryPriceTagDetail;
        _repositoryPackageExtend = repositoryPackageExtend;
        _repositoryVCat = repositoryVCat;
        _repositoryCategory = repositoryCategory;
    }

    public async Task<int> Handle(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var reservation = new Reservation(
                                request.MandantId,
                                request.ResKz,
                                request.BookerId,
                                request.GuestId > 0 ? request.GuestId : null,
                                request.CompanyId > 0 ? request.CompanyId : null,
                                request.CompanyContactId > 0 ? request.CompanyContactId : null,
                                request.TravelAgentId > 0 ? request.TravelAgentId : null,
                                request.TravelAgentContactId > 0 ? request.TravelAgentContactId : null,
                                request.Persons,
                                request.Arrival,
                                request.Departure,
                                request.CategoryId,
                                request.RoomAmount,
                                request.RoomNumberId,
                                request.RoomNumber,
                                request.RoomFixed,
                                request.RateId,
                                request.RatePackages,
                                request.LogisTotal,
                                request.BookingPolicyId,
                                request.CancellationPolicyId,
                                request.IsGroupMaster,
                                request.GroupMasterId,
                                request.Transfer != null ? request.Transfer : null,
                                request.MatchCode,
                                request.OptionDate,
                                request.OptionFollowUp,
                                request.CRSNumber != null ? request.CRSNumber : null,
                                request.PaxString,
                                request.CartId,
                                request.Confirmations,
                                request.Wishes);
        reservation.DomainEvents.Add(EntityCreatedEvent.WithEntity(reservation));
        await _repository.AddAsync(reservation, cancellationToken);

        #region VKat Calculation

        Category category = await _repositoryCategory.GetByIdAsync(reservation.CategoryId, cancellationToken);

        Pax pax = JsonSerializer.Deserialize<Pax>(reservation.PaxString);

        int nights = Convert.ToInt32((reservation.Departure - reservation.Arrival).TotalDays);
        for (int i = 0; i < nights; i++)
        {
            var spec = new VCatByMandantDateCategorySpec(reservation.MandantId, reservation.Arrival.AddDays(i), reservation.CategoryId);
            var vCat = await _repositoryVCat.GetBySpecAsync(spec, cancellationToken);

            int bedsAdult = pax.Adult;
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

            extraBeds += pax.Children.Count(x => x.ExtraBed) * (int)reservation.RoomAmount;

            vCat.RollInByReservation(
                (int)reservation.RoomAmount,
                0,
                i == 0 ? (int)reservation.RoomAmount : 0,
                bedsAdult,
                extraBeds,
                pax.Adult * (int)reservation.RoomAmount,
                pax.Children.Count() * (int)reservation.RoomAmount,
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

        PriceTag pt = new(
            request.PriceTag.MandantId,
            reservation.Id,
            reservation.Arrival,
            reservation.Departure,
            request.PriceTag.AverageRate,
            request.PriceTag.UserRate,
            request.PriceTag.RateSelected,
            request.PriceTag.CategoryId);

        pt.DomainEvents.Add(EntityCreatedEvent.WithEntity(pt));
        await _repositoryPriceTag.AddAsync(pt, cancellationToken);

        foreach (PriceTagDetailDto ptq in request.PriceTag.PriceTagDetails)
        {
            var ptd = new PriceTagDetail(
                pt.Id,
                ptq.CategoryId,
                ptq.RateId,
                ptq.DatePrice,
                ptq.PaxAmount,
                ptq.RateCurrent,
                ptq.RateStart,
                ptq.RateAutomatic,
                ptq.UserRate,
                ptq.AverageRate,
                null,
                ptq.RateTypeEnumId,
                false,
                null);
            ptd.DomainEvents.Add(EntityCreatedEvent.WithEntity(ptd));
            await _repositoryPriceTagDetail.AddAsync(ptd, cancellationToken);
        }

        if (request.PackageExtendList != null && request.PackageExtendList.Count > 0)
        {
            foreach (PackageExtendDto peDto in request.PackageExtendList)
            {
                PackageExtend packageExtend = new(
                    request.MandantId,
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
