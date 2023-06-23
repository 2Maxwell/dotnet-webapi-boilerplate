using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceTags;
using FSH.WebApi.Application.Hotel.RoomReservations;
using FSH.WebApi.Application.Hotel.Rooms;
using FSH.WebApi.Application.Hotel.VCats;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;
using Mapster;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class UpdateReservationRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? ResKz { get; set; } // A = Offer, P = Pending, R = Reservation, C = CheckedIn, O = CheckedOut, I = CartItem
    public int BookerId { get; set; }
    public int? GuestId { get; set; }
    public int? CompanyId { get; set; }
    public int? CompanyContactId { get; set; }
    public int? TravelAgentId { get; set; }
    public int? TravelAgentContactId { get; set; }
    public string? Persons { get; set; }
    //public List<PersonShopItem>? PersonShopItems { get; set; } = new();
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
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
    public string? Confirmations { get; set; }
    public string? Wishes { get; set; }
    public PriceTagDto? PriceTagDto { get; set; }
    public List<PackageExtendDto>? PackageExtendOptionDtos { get; set; } = new();
}

public class UpdateReservationRequestValidator : CustomValidator<UpdateReservationRequest>
{
    public UpdateReservationRequestValidator(IReadRepository<Reservation> repository, IStringLocalizer<UpdateReservationRequestValidator> localizer)
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

public class UpdateReservationRequestHandler : IRequestHandler<UpdateReservationRequest, int>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IStringLocalizer<UpdateReservationRequestHandler> _localizer;
    private readonly IRepository<PriceTag> _repositoryPriceTag;
    private readonly IRepository<PriceTagDetail> _repositoryPriceTagDetail;
    private readonly IRepository<VCat> _repositoryVCat;
    private readonly IRepository<Category> _repositoryCategory;
    private readonly IRepository<RoomReservation> _repositoryRoomReservation;
    private readonly IRepository<Mandant> _repositoryMandant;
    private readonly IRepositoryWithEvents<Room> _repositoryRoom;

    public UpdateReservationRequestHandler(IRepository<Reservation> repository, IStringLocalizer<UpdateReservationRequestHandler> localizer, IRepository<PriceTag> repositoryPriceTag, IRepository<PriceTagDetail> repositoryPriceTagDetail, IRepository<VCat> repositoryVCat, IRepository<Category> repositoryCategory, IRepository<RoomReservation> repositoryRoomReservation, IRepository<Mandant> repositoryMandant, IRepositoryWithEvents<Room> repositoryRoom)
    {
        _repository = repository;
        _localizer = localizer;
        _repositoryPriceTag = repositoryPriceTag;
        _repositoryPriceTagDetail = repositoryPriceTagDetail;
        _repositoryVCat = repositoryVCat;
        _repositoryCategory = repositoryCategory;
        _repositoryRoomReservation = repositoryRoomReservation;
        _repositoryMandant = repositoryMandant;
        _repositoryRoom = repositoryRoom;
    }

    public async Task<int> Handle(UpdateReservationRequest request, CancellationToken cancellationToken)
    {
        // Console Ausgabe Start UpdateReservationRequestHandler
        Console.WriteLine("Start UpdateReservationRequestHandler");

        var reservation = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = reservation ?? throw new NotFoundException(string.Format(_localizer["reservation.notfound"], request.Id));

        ReservationHelper reservationHelper = new ReservationHelper();

        Category category = await _repositoryCategory.GetByIdAsync(reservation.CategoryId, cancellationToken);

        // Mandant laden und HotelDate laden
        Mandant mandant = await _repositoryMandant.GetByIdAsync(reservation.MandantId, cancellationToken);

        // Console Mandant geladen
        Console.WriteLine("Mandant geladen");

        string updateCase = reservationHelper.getResKzUpdateCase(reservation.ResKz, request.ResKz);

        // Console Ausgabe updateCase
        Console.WriteLine("updateCase: " + updateCase);

        string executeWhen = "Update, P_R, P_C, P_S, R_C, R_S C_R, C_O,";
        if (executeWhen.Contains(updateCase + ","))
        {
            #region VKat Calculation RollBackReservation

            // Console Ausgabe VKat Calculation RollBackReservation
            Console.WriteLine("VKat Calculation RollBackReservation wird ausgeführt " + updateCase);


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

                vCat.RollBackByReservation(
                    (int)reservation.RoomAmount,
                    i == 0 ? 0 : (int)reservation.RoomAmount,
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
            vCatDeparture.RollBackByReservation(
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

            #region RoomReservation

            // Console Ausgabe RoomReservation wird ausgeführt
            Console.WriteLine("RoomReservation wird ausgeführt " + updateCase);

            // Room vergleichen
            // wenn neu vorhanden => eintragen
            // wenn nichts geändert inkl. Anreise und Abreise => nichts tun
            // wenn geändert => altes Zimmer anhand der ResId freigeben und neues eintragen

            // die executeWhen Fälle ansehen denn bei Storno muss das Zimmer nur freigegeben werden
            // erledigt MW 02.06.23

            string roomExecution = reservationHelper.getRoomUpdateCase(reservation.RoomNumberId, request.RoomNumberId, reservation.Arrival, Convert.ToDateTime(request.Arrival), reservation.Departure, Convert.ToDateTime(request.Departure));

            if (roomExecution == "newRoom" && !updateCase.Contains("_S,"))
            {
                // CreateRoomReservationRequest
                // Console Ausgabe CreateRoomReservationRequest mit updateCase und roomExecution
                Console.WriteLine("CreateRoomReservationRequest wird ausgeführt " + updateCase + " " + roomExecution);
                CreateRoomReservationRequest createRoomRequest = new CreateRoomReservationRequest()
                {
                    MandantId = reservation.MandantId,
                    RoomId = request.RoomNumberId,
                    Name = request.RoomNumber,
                    Arrival = Convert.ToDateTime(request.Arrival),
                    Departure = Convert.ToDateTime(request.Departure),
                    ReservationId = request.Id
                };
                CreateRoomReservationRequestHandler createRoomReservationRequestHandler = new CreateRoomReservationRequestHandler(_repositoryRoomReservation);
                int lines = await createRoomReservationRequestHandler.Handle(createRoomRequest, cancellationToken);

            }

            if (roomExecution == "deleteRoom")
            {

                // DeleteRommReservationRequest
                // Console Ausgabe DeleteRommReservationRequest mit updateCase und roomExecution
                Console.WriteLine("DeleteRommReservationRequest wird ausgeführt " + updateCase + " " + roomExecution);
                DeleteRoomReservationRequest deleteRoomRequest = new DeleteRoomReservationRequest()
                {
                    MandantId = reservation.MandantId,
                    ReservationId = request.Id
                };
                DeleteRoomReservationRequestHandler deleteRoomReservationRequestHandler = new DeleteRoomReservationRequestHandler(_repositoryRoomReservation);
                int linesDelete = await deleteRoomReservationRequestHandler.Handle(deleteRoomRequest, cancellationToken);
            }

            if (roomExecution == "changeRoom")
            {
                // DeleteRommReservationRequest
                // Console Ausgabe DeleteRommReservationRequest mit updateCase und roomExecution
                Console.WriteLine("DeleteRommReservationRequest wird ausgeführt " + updateCase + " " + roomExecution);

                DeleteRoomReservationRequest deleteRoomRequest = new DeleteRoomReservationRequest()
                {
                    MandantId = reservation.MandantId,
                    ReservationId = request.Id
                };
                DeleteRoomReservationRequestHandler deleteRoomReservationRequestHandler = new DeleteRoomReservationRequestHandler(_repositoryRoomReservation);
                int linesDelete = await deleteRoomReservationRequestHandler.Handle(deleteRoomRequest, cancellationToken);

                // CreateRoomReservationRequest wenn updateCase nicht Storno
                if (!updateCase.Contains("_S,"))
                {
                    Console.WriteLine("CreateRoomReservationRequest wird ausgeführt " + updateCase + " " + roomExecution);
                    CreateRoomReservationRequest createRoomRequest = new CreateRoomReservationRequest()
                    {
                        MandantId = reservation.MandantId,
                        RoomId = request.RoomNumberId,
                        Name = request.RoomNumber,
                        Arrival = Convert.ToDateTime(request.Arrival),
                        Departure = Convert.ToDateTime(request.Departure),
                        ReservationId = request.Id
                    };
                    CreateRoomReservationRequestHandler createRoomReservationRequestHandler = new CreateRoomReservationRequestHandler(_repositoryRoomReservation);
                    int lines = await createRoomReservationRequestHandler.Handle(createRoomRequest, cancellationToken);
                }
            }

            #endregion
        }

        executeWhen = "A_C, P_C, R_C,";
        if (executeWhen.Contains(updateCase + ","))
        {
            // CheckIn
            // Room verwalten Stati setzen
            // Arrival Checkin = true
            // Occupied = true
            // Room dirty
            // Vereinbarung über Zimmerreinigung setzen

        }

        var updatedReservation = reservation.Update(
                        request.ResKz,
                        request.BookerId,
                        request.GuestId,
                        request.CompanyId,
                        request.CompanyContactId,
                        request.TravelAgentId,
                        request.TravelAgentContactId,
                        request.Persons,
                        request.Arrival!.Value,
                        request.Departure!.Value,
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
                        request.Transfer,
                        request.MatchCode,
                        request.OptionDate,
                        request.OptionFollowUp,
                        request.CRSNumber,
                        request.PaxString,
                        request.CartId,
                        request.Confirmations,
                        request.Wishes);
        reservation.DomainEvents.Add(EntityUpdatedEvent.WithEntity(reservation));
        await _repository.UpdateAsync(updatedReservation, cancellationToken);

        executeWhen = "Update, P_R, P_C, R_C, C_R, C_O, O_C";
        if (executeWhen.Contains(updateCase + ","))
        {
            #region VKat Calculation RollInReservation

            if (updatedReservation.ResKz != "S") // S = Storno darf nicht mit RoolBack berechnet werden
            {
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
                        i == 0 ? 0 : (int)reservation.RoomAmount,
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
            }

            #endregion
        }

        executeWhen = "Update, P_R, P_C, R_C, C_R, C_O, O_C, S_P, S_R";
        if (executeWhen.Contains(updateCase + ","))
        {
            #region RoomState setzen


            // RoomState setzen
            // Wenn Reservation.RoomId > 0 & Reservation.Arrival = Mandant.Hoteldate RoomState = ArrExp true
            if (reservation.RoomNumberId > 0 && reservation.Arrival.Date == mandant.HotelDate)
            {
                SetRoomStateArrivalExpectedRequest setRoomStateArrivalExpectedRequest = new SetRoomStateArrivalExpectedRequest()
                {
                    Id = reservation.RoomNumberId,
                    MandantId = reservation.MandantId
                };

                SetRoomStateArrivalExpectedRequestHandler setRoomStateArrivalExpectedHandler = new (_repositoryRoom);
                var roomId = await setRoomStateArrivalExpectedHandler.Handle(setRoomStateArrivalExpectedRequest, cancellationToken);
            }

            if (updateCase.Contains("_C") && reservation.RoomNumberId > 0)
            {
                // RoomStateCheckInRequest setzen
                SetRoomStateCheckInRequest setRoomStateCheckInRequest = new SetRoomStateCheckInRequest()
                {
                    Id = reservation.RoomNumberId,
                    MandantId = reservation.MandantId
                };

                SetRoomStateCheckInRequestHandler setRoomStateCheckInHandler = new (_repositoryRoom);
                var roomId = await setRoomStateCheckInHandler.Handle(setRoomStateCheckInRequest, cancellationToken);
            }

            if (updateCase.Contains("_O"))
            {
                // RoomStateCheckOutRequest setzen

                SetRoomStateCheckOutRequest setRoomStateCheckOutRequest = new SetRoomStateCheckOutRequest()
                {
                    Id = reservation.RoomNumberId,
                    MandantId = reservation.MandantId
                };

                SetRoomStateCheckOutRequestHandler setRoomStateCheckOutHandler = new (_repositoryRoom);
                var roomId = await setRoomStateCheckOutHandler.Handle(setRoomStateCheckOutRequest, cancellationToken);

                // bool done = await _repositoryRoomState.SetCheckOutAsync(reservation.RoomNumberId, true, cancellationToken);
            }

            #endregion
        }

        executeWhen = "C_O,";
        if (executeWhen.Contains(updateCase + ","))
        {
            // CheckOut
            // Room verwalten Stati setzen
            // CheckOut Occupied = false
            // Departure Out = true
            // Room Dirty
            // Occupied = true
        }

        #region PriceTag PriceTagDetail

        if (request.PriceTagDto.Id > 0)
        {
            var priceTag = await _repositoryPriceTag.GetByIdAsync(request.PriceTagDto.Id);
            priceTag.Update(
                request.PriceTagDto.ReservationId,
                request.PriceTagDto.Arrival,
                request.PriceTagDto.Departure,
                request.PriceTagDto.AverageRate,
                request.PriceTagDto.UserRate,
                request.PriceTagDto.RateSelected,
                request.PriceTagDto.CategoryId);
            priceTag.DomainEvents.Add(EntityUpdatedEvent.WithEntity(priceTag));
            await _repositoryPriceTag.UpdateAsync(priceTag, cancellationToken);

        }
        else
        {
            var priceTag = new PriceTag(
                                        request.MandantId,
                                        request.PriceTagDto.ReservationId,
                                        request.PriceTagDto.Arrival,
                                        request.PriceTagDto.Departure,
                                        request.PriceTagDto.AverageRate,
                                        request.PriceTagDto.UserRate,
                                        request.PriceTagDto.RateSelected,
                                        request.CategoryId);
            priceTag.DomainEvents.Add(EntityCreatedEvent.WithEntity(priceTag));
            await _repositoryPriceTag.AddAsync(priceTag, cancellationToken);

        }

        foreach (PriceTagDetailDto tagDetail in request.PriceTagDto.PriceTagDetails!)
        {
            if (tagDetail.Id > 0 && tagDetail.PriceTagId > 0)
            {
                var priceTagDetail = await _repositoryPriceTagDetail.GetByIdAsync(tagDetail.Id, cancellationToken);
                if (priceTagDetail != null)
                {
                    priceTagDetail.Update(
                                    Convert.ToDecimal(request.PriceTagDto.UserRate),
                                    Convert.ToDecimal(request.PriceTagDto.AverageRate),
                                    tagDetail.NoShow,
                                    tagDetail.NoShowPercentage);
                    priceTagDetail.DomainEvents.Add(EntityUpdatedEvent.WithEntity(priceTagDetail));
                    await _repositoryPriceTagDetail.UpdateAsync(priceTagDetail, cancellationToken);

                }

            }

            if (tagDetail.Id == 0 && tagDetail.PriceTagId > 0)
            {
                PriceTagDetail ptd = new PriceTagDetail(
                                            request.PriceTagDto.Id,
                                            request.PriceTagDto.CategoryId,
                                            tagDetail.RateId,
                                            tagDetail.DatePrice,
                                            tagDetail.PaxAmount,
                                            tagDetail.RateCurrent,
                                            tagDetail.RateStart,
                                            tagDetail.RateAutomatic,
                                            Convert.ToDecimal(request.PriceTagDto.UserRate),
                                            Convert.ToDecimal(request.PriceTagDto.AverageRate),
                                            tagDetail.EventDates,
                                            tagDetail.RateTypeEnumId,
                                            tagDetail.NoShow,
                                            tagDetail.NoShowPercentage);
                ptd.DomainEvents.Add(EntityCreatedEvent.WithEntity(ptd));
                await _repositoryPriceTagDetail.AddAsync(ptd, cancellationToken);
            }

            if (tagDetail.Id > 0 && tagDetail.PriceTagId < 0)
            {
                var priceTagDetail = await _repositoryPriceTagDetail.GetByIdAsync(tagDetail.Id, cancellationToken);
                if (priceTagDetail != null)
                {
                    // priceTagDetail.Delete(tagDetail.PriceTagId);
                    await _repositoryPriceTagDetail.DeleteAsync(priceTagDetail, cancellationToken);
                }
            }

        }

        #endregion

        return request.Id;
    }
}
