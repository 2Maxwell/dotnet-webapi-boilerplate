using FSH.WebApi.Application.Accounting;
using FSH.WebApi.Application.Hotel.Reservations;
using FSH.WebApi.Application.Hotel.RoomReservations;
using FSH.WebApi.Application.Hotel.Rooms;
using FSH.WebApi.Application.Hotel.VCats;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.General;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.General;
public class GetNightAuditRequest : IRequest<GetNightAuditResponse>
{
    public GetNightAuditRequest(int mandantId, DateTime? date)
    {
        MandantId = mandantId;
        Date = date;
    }

    public int MandantId { get; set; }
    public DateTime? Date { get; set; }
}

public class GetNightAuditHandler : IRequestHandler<GetNightAuditRequest, GetNightAuditResponse>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IRepository<RoomReservation> _repositoryRoomReservation;
    private readonly IRepository<PriceTag> _repositoryPriceTag;
    private readonly IRepository<PackageExtend> _repositoryPackageExtend;
    private readonly IRepository<Package> _repositoryPackage;
    private readonly IRepository<PackageItem> _repositoryPackageItem;
    private readonly IRepository<Booking> _repositoryBooking;
    private readonly IRepository<Appointment> _repositoryAppointment;
    private readonly IRepository<MandantSetting> _repositoryMandantSetting;
    private readonly IRepository<Mandant> _repositoryMandant;
    private readonly IReadRepository<Tax> _repositoryTax;
    private readonly IRepository<TaxItem> _repositoryTaxItem;
    private readonly IReadRepository<Item> _repositoryItem;
    private readonly IRepository<ItemPriceTax> _repositoryItemPriceTax;
    private readonly IRepository<Journal> _repositoryJournal;
    private readonly IRepository<MandantNumbers> _repositoryMandantNumbers;
    private readonly IRepository<CashierJournal> _repositoryCashierJournal;
    private readonly IRepository<Room> _repositoryRoom;
    private readonly IRepository<VCat> _repositoryVCat;
    private readonly IReadRepository<Category> _repositoryCategory;
    private readonly IDapperRepository _repositoryDapper;
    private readonly IStringLocalizer<GetNightAuditHandler> _localizer;

    public GetNightAuditHandler(IRepository<Reservation> repository, IRepository<RoomReservation> repositoryRoomReservation, IRepository<PriceTag> repositoryPriceTag, IRepository<PackageExtend> repositoryPackageExtend, IRepository<Package> repositoryPackage, IRepository<PackageItem> repositoryPackageItem, IRepository<Booking> repositoryBooking, IRepository<Appointment> repositoryAppointment, IRepository<MandantSetting> repositoryMandantSetting, IRepository<Mandant> repositoryMandant, IReadRepository<Tax> repositoryTax, IRepository<TaxItem> repositoryTaxItem, IReadRepository<Item> repositoryItem, IRepository<ItemPriceTax> repositoryItemPriceTax, IRepository<Journal> repositoryJournal, IRepository<MandantNumbers> repositoryMandantNumbers, IRepository<CashierJournal> repositoryCashierJournal, IRepository<Room> repositoryRoom, IRepository<VCat> repositoryVCat, IReadRepository<Category> repositoryCategory, IDapperRepository repositoryDapper, IStringLocalizer<GetNightAuditHandler> localizer)
    {
        _repository = repository;
        _repositoryRoomReservation = repositoryRoomReservation;
        _repositoryPriceTag = repositoryPriceTag;
        _repositoryPackageExtend = repositoryPackageExtend;
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _repositoryBooking = repositoryBooking;
        _repositoryAppointment = repositoryAppointment;
        _repositoryMandantSetting = repositoryMandantSetting;
        _repositoryMandant = repositoryMandant;
        _repositoryTax = repositoryTax;
        _repositoryTaxItem = repositoryTaxItem;
        _repositoryItem = repositoryItem;
        _repositoryItemPriceTax = repositoryItemPriceTax;
        _repositoryJournal = repositoryJournal;
        _repositoryMandantNumbers = repositoryMandantNumbers;
        _repositoryCashierJournal = repositoryCashierJournal;
        _repositoryRoom = repositoryRoom;
        _repositoryVCat = repositoryVCat;
        _repositoryCategory = repositoryCategory;
        _repositoryDapper = repositoryDapper;
        _localizer = localizer;
    }

    public async Task<GetNightAuditResponse> Handle(GetNightAuditRequest request, CancellationToken cancellationToken)
    {
        Mandant? mandant = await _repositoryMandant.GetByIdAsync(request.MandantId, cancellationToken);

        // GetNightAuditHandler getNightAuditHandler = new GetNightAuditHandler(_repository, _localizer);
        GetNightAuditResponse getNightAuditResponse = new GetNightAuditResponse();
        getNightAuditResponse.MandantId = request.MandantId;
        getNightAuditResponse.Date = request.Date;

        // nicht angereiste Reservierungen stornieren
        SetNonArrivalToCancelRequest setNonArrivalToCancelRequest = new SetNonArrivalToCancelRequest(request.MandantId, request.Date);
        SetNonArrivalToCancelHandler setNonArrivalToCancelHandler = new SetNonArrivalToCancelHandler(_repository, _repositoryRoomReservation);
        getNightAuditResponse.Result += await setNonArrivalToCancelHandler.Handle(setNonArrivalToCancelRequest, cancellationToken);

        // nicht abgereiste Reservierungen verlängern
        SetNonDepartureToDepartureNextDayRequest setNonDepartureToDepartureNextDayRequest = new SetNonDepartureToDepartureNextDayRequest(request.MandantId, request.Date);
        SetNonDepartureToDepartureNextDayHandler setNonDepartureToDepartureNextDayHandler = new SetNonDepartureToDepartureNextDayHandler(_repository, _repositoryRoomReservation);
        getNightAuditResponse.Result += await setNonDepartureToDepartureNextDayHandler.Handle(setNonDepartureToDepartureNextDayRequest, cancellationToken);

        // Alle Reservierungen mit ResKz = C (GastimHaus) laden
        GetReservationsByResKzRequest getReservationsByResKzRequest = new GetReservationsByResKzRequest(request.MandantId, "C", null, null);
        GetReservationsByResKzRequestHandler getReservationsByResKzHandler = new GetReservationsByResKzRequestHandler(_repository, _repositoryPriceTag, _repositoryPackageExtend, _repositoryPackage, _repositoryPackageItem, _repositoryBooking, _repositoryAppointment);
        List<ReservationDto> reservations = await getReservationsByResKzHandler.Handle(getReservationsByResKzRequest, cancellationToken);

        // Leistungen buchen je Zimmer bzw. je Reservierung im Haus auf Transfer achten und gleich umbuchen Umsatz sollte aber auf dem Zimmer bleiben für Statistik
        List<BookingLine> bookingLines = new List<BookingLine>();
        foreach (ReservationDto res in reservations.Where(x => !x.RoomNumber!.StartsWith("GM")))
        {
            if (res.BookingDone is null || (res.BookingDone.Value.Date < request.Date.Value.Date))
            {
                GetReservationNightAuditBookingRequest getReservationNightAuditBookingRequest = new GetReservationNightAuditBookingRequest(request.Date, res);
                GetReservationNightAuditBookingRequestHandler getReservationNightAuditBookingHandler = new GetReservationNightAuditBookingRequestHandler(_repositoryMandantSetting, _repositoryTax, _repositoryTaxItem, _repositoryItem, _repositoryItemPriceTax, _repositoryPackage, _repositoryPackageItem, _repositoryBooking, _repositoryJournal, _repositoryMandantNumbers, _repositoryCashierJournal);
                bookingLines.AddRange(await getReservationNightAuditBookingHandler.Handle(getReservationNightAuditBookingRequest, cancellationToken));

                UpdateReservationBookingDoneRequest updateReservationBookingDoneRequest = new UpdateReservationBookingDoneRequest(res.Id, Convert.ToDateTime(request.Date));
                UpdateReservationBookingDoneRequestHandler updateReservationBookingDoneRequestHandler = new UpdateReservationBookingDoneRequestHandler(_repository);
                int resId = await updateReservationBookingDoneRequestHandler.Handle(updateReservationBookingDoneRequest, cancellationToken);

                getNightAuditResponse.Result += "ReservationId: " + res.Id + " BookingDone: " + Convert.ToDateTime(request.Date) + "\r\n";
            }
        }

        // Zimmerstatus setzen
        foreach (ReservationDto res in reservations)
        {
            if (!res.RoomNumber!.StartsWith("GM") || !res.RoomNumber.StartsWith("PZ"))
            {

                Room? room = await _repositoryRoom.GetByIdAsync(res.RoomNumberId, cancellationToken);

                if (room != null)
                {
                    room.Clean = false;
                    room.ArrExp = false;
                    room.ArrCi = false;
                    room.Occ = true;
                    room.DepExp = res.Departure >= mandant.HotelDate.AddDays(1) && res.Departure < mandant.HotelDate.AddDays(2);  // TODO: prüfen ob diese Bedingung funktioniert.
                    room.DepOut = false;
                    room.DirtyDays = room.DirtyDays++;
                    room.CleaningState = 0;
                    await _repositoryRoom.UpdateAsync(room, cancellationToken);
                }
            }
        }

        // TODO: Statistiken erzeugen
        // TODO: Transfer durchführen
        // TODO: History für Abreisen erzeugen
        // TODO: Reporte erzeugen
        // TODO: TSE Daten erzeugen
        // TODO: Monatsabschluss wenn notwendig erzeugen
        // TODO: Jahresabschluss wenn notwendig erzeugen

        // TODO: neues HotelDatum in Mandant setzen, aktuelle Datum + 1
        
        mandant!.HotelDate = mandant.HotelDate.AddDays(1);
        await _repositoryMandant.UpdateAsync(mandant, cancellationToken);
        getNightAuditResponse.Result += "HotelDate: " + mandant.HotelDate + "\r\n";

        // TODO: VKAT neu berechnen, fehlende Tage ergänzen
        CalculateVCatRequest calculateVCatRequest = new CalculateVCatRequest(request.MandantId);
        CalculateVCatRequestHandler calculateVCatRequestHandler = new CalculateVCatRequestHandler(_repositoryVCat, _repositoryMandant, _repositoryMandantSetting, _repositoryCategory, _repositoryRoom, _repository, _repositoryDapper);
        getNightAuditResponse.Result += await calculateVCatRequestHandler.Handle(calculateVCatRequest, cancellationToken);

        // TODO: abgelaufene Optionen Message erzeugen

        return getNightAuditResponse;
    }

    // TODO: - nicht angereiste Reservierungen stornieren kontrollieren ob Deposit vorhanden
    // Leistungen buchen je Zimmer bzw. je Reservierung im Haus auf Transfer achten und gleich umbuchen Umsatz sollte aber auf dem Zimmer bleiben für Statistik

}

public class GetNightAuditResponse
{
    public int MandantId { get; set; }
    public DateTime? Date { get; set; }
    public string Result { get; set; } = string.Empty;
}

public class SetNonArrivalToCancelRequest : IRequest<string>
{
    public SetNonArrivalToCancelRequest(int mandantId, DateTime? date)
    {
        MandantId = mandantId;
        Date = date;
    }

    public int MandantId { get; set; }
    public DateTime? Date { get; set; }
}

public class SetNonArrivalToCancelHandler : IRequestHandler<SetNonArrivalToCancelRequest, string>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IRepository<RoomReservation> _repositoryRoomReservation;

    public SetNonArrivalToCancelHandler(IRepository<Reservation> repository, IRepository<RoomReservation> repositoryRoomReservation)
    {
        _repository = repository;
        _repositoryRoomReservation = repositoryRoomReservation;
    }

    public async Task<string> Handle(SetNonArrivalToCancelRequest request, CancellationToken cancellationToken)
    {
        string result = "SetNonArrivalToCancel: " + "\r\n";
        var reservations = (await _repository.ListAsync(cancellationToken))
            .Where(x => x.MandantId == request.MandantId
                            && x.Arrival.Date == Convert.ToDateTime(request.Date).Date
                            && (x.ResKz == "P" | x.ResKz == "R")).ToList();

        foreach (Reservation res in reservations)
        {
            result += res.Id + "\r\n";
            res.ResKz = "S";
            await _repository.UpdateAsync(res, cancellationToken);

            if (res.RoomNumberId > 0)
            {
                DeleteRoomReservationRequest deleteRoomRequest = new DeleteRoomReservationRequest()
                {
                    MandantId = res.MandantId,
                    ReservationId = res.Id
                };
                DeleteRoomReservationRequestHandler deleteRoomReservationRequestHandler = new DeleteRoomReservationRequestHandler(_repositoryRoomReservation);
                int linesDelete = await deleteRoomReservationRequestHandler.Handle(deleteRoomRequest, cancellationToken);
            }

        }

        return result;
    }
}

public class SetNonDepartureToDepartureNextDayRequest : IRequest<string>
{
    public SetNonDepartureToDepartureNextDayRequest(int mandantId, DateTime? date)
    {
        MandantId = mandantId;
        Date = date;
    }

    public int MandantId { get; set; }
    public DateTime? Date { get; set; }
}

public class SetNonDepartureToDepartureNextDayHandler : IRequestHandler<SetNonDepartureToDepartureNextDayRequest, string>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IRepository<RoomReservation> _repositoryRoomReservation;

    public SetNonDepartureToDepartureNextDayHandler(IRepository<Reservation> repository, IRepository<RoomReservation> repositoryRoomReservation)
    {
        _repository = repository;
        _repositoryRoomReservation = repositoryRoomReservation;
    }

    public async Task<string> Handle(SetNonDepartureToDepartureNextDayRequest request, CancellationToken cancellationToken)
    {
        string result = "SetNonDepartureToDepartureNextDay: " + "\r\n";
        var reservations = (await _repository.ListAsync(cancellationToken))
            .Where(x => x.MandantId == request.MandantId
                                    && x.Departure.Date == Convert.ToDateTime(request.Date).Date
                                    && x.ResKz == "C").ToList();

        foreach (Reservation res in reservations)
        {
            result += res.Id + "\r\n";
            res.Departure.AddDays(1);
            await _repository.UpdateAsync(res, cancellationToken);

            if (res.RoomNumberId > 0)
            {
                DeleteRoomReservationRequest deleteRoomRequest = new DeleteRoomReservationRequest()
                {
                    MandantId = res.MandantId,
                    ReservationId = res.Id
                };
                DeleteRoomReservationRequestHandler deleteRoomReservationRequestHandler = new DeleteRoomReservationRequestHandler(_repositoryRoomReservation);
                int linesDelete = await deleteRoomReservationRequestHandler.Handle(deleteRoomRequest, cancellationToken);

                CreateRoomReservationRequest createRoomRequest = new CreateRoomReservationRequest()
                {
                    MandantId = res.MandantId,
                    RoomId = res.RoomNumberId,
                    Name = res.RoomNumber,
                    Arrival = Convert.ToDateTime(res.Arrival),
                    Departure = Convert.ToDateTime(res.Departure),
                    ReservationId = res.Id
                };
                CreateRoomReservationRequestHandler createRoomReservationRequestHandler = new CreateRoomReservationRequestHandler(_repositoryRoomReservation);
                int lines = await createRoomReservationRequestHandler.Handle(createRoomRequest, cancellationToken);

            }

        }

        return result;
    }
}