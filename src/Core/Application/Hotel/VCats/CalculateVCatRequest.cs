using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Application.Hotel.Rooms;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace FSH.WebApi.Application.Hotel.VCats;
public class CalculateVCatRequest : IRequest<int>
{
    public CalculateVCatRequest(int mandantId)
    {
        MandantId = mandantId;
    }

    public int MandantId { get; set; }
}

public class CalculateVCatRequestHandler : IRequestHandler<CalculateVCatRequest, int>
{
    private readonly IRepository<VCat> _repository;
    private readonly IReadRepository<Mandant> _repositoryMandant;
    private readonly IReadRepository<MandantSetting> _repositoryMandantSetting;
    private readonly IReadRepository<Category> _categoryRepository;
    private readonly IReadRepository<Room> _roomRepository;
    private readonly IReadRepository<Reservation> _reservationRepository;
    private readonly IDapperRepository _dapperRepository;

    public CalculateVCatRequestHandler(IRepository<VCat> repository, IReadRepository<Mandant> repositoryMandant, IReadRepository<MandantSetting> repositoryMandantSetting, IReadRepository<Category> categoryRepository, IReadRepository<Room> roomRepository, IReadRepository<Reservation> reservationRepository, IDapperRepository dapperRepository)
    {
        _repository = repository;
        _repositoryMandant = repositoryMandant;
        _repositoryMandantSetting = repositoryMandantSetting;
        _categoryRepository = categoryRepository;
        _roomRepository = roomRepository;
        _reservationRepository = reservationRepository;
        _dapperRepository = dapperRepository;
    }

    public async Task<int> Handle(CalculateVCatRequest request, CancellationToken cancellationToken)
    {
        // var specMandant = new MandantByIdSpec(request.MandantId);
        Mandant mandant = await _repositoryMandant.GetByIdAsync(request.MandantId, cancellationToken);
        DateTime Hoteldate = mandant.HotelDate;

        MandantSetting mandantSetting = await _repositoryMandantSetting.GetByIdAsync(request.MandantId, cancellationToken);
        int vcatDays = mandantSetting.ForecastDays;

        var specVCat = new VCatsByMandantIdSpec(request.MandantId, Hoteldate);
        List<VCat> vCats = await _repository.ListAsync(specVCat, cancellationToken);

        var specCategory = new CategorysByMandantIdSpec(request.MandantId);
        List<Category> categories = await _categoryRepository.ListAsync(specCategory, cancellationToken);

        var specRoom = new RoomsByMandantIdSpec(request.MandantId);
        List<Room> rooms = await _roomRepository.ListAsync(specRoom, cancellationToken);

        int counter = 0;

        // für alle Categorys und alle Tage die VCat laden oder neu erzeugen
        foreach (Category category in categories.Where(x => !x.CategoryIsVirtual))
        {
            for (int i = 0; i < vcatDays; i++)
            {
                DateTime workDate = Hoteldate.AddDays(i);

                VCat? vCat = vCats.FirstOrDefault(x => x.Date == workDate && x.CategoryId == category.Id);

                if (vCat != null)
                {

                    // TODO Überlegung: BedsBlocked und ExtraBedsBlocked zählen und von BedsInventory abziehen.

                    // alle Rooms zählen die zur Category gehören
                    vCat.Amount = rooms.Where(x => x.MandantId == request.MandantId && x.CategoryId == category.Id).Count();

                    // alle Beds in Rooms zählen die zur Category gehören und in BedInventory eintragen
                    // vCat.BedsInventory = rooms.Where(x => x.CategoryId == category.Id).Sum(x => x.Beds);

                    // alle Beds in Rooms zählen die zur Category gehören und die am Tag Blocked sind oder BlockedStart null und und BlockedEnd > workDate oder BlockedStart <= workDate und BlockedEnd null oder BlockedStart und BlockedEnd null in BedInventory eintragen
                    // vCat.BedsInventory = rooms.Where(x => x.CategoryId == category.Id && ((x.Blocked && x.BlockedStart <= workDate && x.BlockedEnd > workDate) || (x.BlockedStart == null && x.BlockedEnd > workDate) || (x.BlockedStart <= workDate && x.BlockedEnd == null) || (x.BlockedStart == null && x.BlockedEnd == null))).Sum(x => x.Beds);
                    vCat.BedsInventory = rooms.Where(x => x.MandantId == request.MandantId && x.CategoryId == category.Id
                    && !((x.Blocked && ((x.BlockedStart <= workDate && x.BlockedEnd > workDate) || (x.BlockedStart == null && x.BlockedEnd > workDate) || (x.BlockedStart <= workDate && x.BlockedEnd == null) || (x.BlockedStart == null && x.BlockedEnd == null)))
                    || (!x.Blocked && ((x.BlockedStart <= workDate && x.BlockedEnd > workDate) || (x.BlockedStart == null && x.BlockedEnd > workDate) || (x.BlockedStart <= workDate && x.BlockedEnd == null)))))
                    .Sum(x => x.Beds);


                    // alle BedsExtra in Rooms zählen die zur Category gehören und in ExtraBedsInventory eintragen
                    // vCat.ExtraBedsInventory = rooms.Where(x => x.CategoryId == category.Id).Sum(x => x.BedsExtra);

                    // alle BedsExtra in Rooms zählen die zur Category gehören und die am Tag Blocked sind oder BlockedStart null und und BlockedEnd > workDate oder BlockedStart <= workDate und BlockedEnd null oder BlockedStart und BlockedEnd null in ExtraBedsInventory eintragen
                    // vCat.ExtraBedsInventory = rooms.Where(x => x.CategoryId == category.Id && ((x.Blocked && x.BlockedStart <= workDate && x.BlockedEnd > workDate) || (x.BlockedStart == null && x.BlockedEnd > workDate) || (x.BlockedStart <= workDate && x.BlockedEnd == null) || (x.BlockedStart == null && x.BlockedEnd == null))).Sum(x => x.BedsExtra);
                    vCat.ExtraBedsInventory = rooms.Where(x => x.MandantId == request.MandantId && x.CategoryId == category.Id
                    && !((x.Blocked && ((x.BlockedStart <= workDate && x.BlockedEnd > workDate) || (x.BlockedStart == null && x.BlockedEnd > workDate) || (x.BlockedStart <= workDate && x.BlockedEnd == null) || (x.BlockedStart == null && x.BlockedEnd == null)))
                    || (!x.Blocked && ((x.BlockedStart <= workDate && x.BlockedEnd > workDate) || (x.BlockedStart == null && x.BlockedEnd > workDate) || (x.BlockedStart <= workDate && x.BlockedEnd == null)))))
                    .Sum(x => x.BedsExtra);


                    // alle Rooms zählen die zur Category gehören und die am Tag Blocked sind
                    // vCat.Blocked = rooms.Where(x => x.CategoryId == category.Id && x.Blocked && x.BlockedStart <= workDate && x.BlockedEnd > workDate).Count();

                    // alle Rooms zählen die zur Category gehören und die am Tag Blocked sind oder BlockedStart null und und BlockedEnd > workDate oder BlockedStart <= workDate und BlockedEnd null
                    // vCat.Blocked = rooms.Where(x => x.CategoryId == category.Id && ((x.Blocked && x.BlockedStart <= workDate && x.BlockedEnd > workDate) || (x.BlockedStart == null && x.BlockedEnd > workDate) || (x.BlockedStart <= workDate && x.BlockedEnd == null))).Count();

                    // alle Rooms zählen die zur Category gehören und die am Tag Blocked sind oder BlockedStart null und und BlockedEnd > workDate oder BlockedStart <= workDate und BlockedEnd null oder BlockedStart und BlockedEnd null
                    // vCat.Blocked = rooms.Count(x => x.CategoryId == category.Id && x.Blocked && (x.BlockedStart <= workDate && x.BlockedEnd > workDate || x.BlockedStart == null && x.BlockedEnd > workDate || x.BlockedStart <= workDate && x.BlockedEnd == null || x.BlockedStart == null && x.BlockedEnd == null));
                    vCat.Blocked = rooms.Count(x => x.MandantId == request.MandantId && x.CategoryId == category.Id
                    && ((x.Blocked && ((x.BlockedStart <= workDate && x.BlockedEnd > workDate) || (x.BlockedStart == null && x.BlockedEnd > workDate) || (x.BlockedStart <= workDate && x.BlockedEnd == null) || (x.BlockedStart == null && x.BlockedEnd == null)))
                    || (!x.Blocked && ((x.BlockedStart <= workDate && x.BlockedEnd > workDate) || (x.BlockedStart == null && x.BlockedEnd > workDate) || (x.BlockedStart <= workDate && x.BlockedEnd == null)))));

                    // alle Reservations zählen die zur Category gehören und die am Tag ankommen
                    // vCat.Arrival = await _reservationRepository.CountAsync(new ReservationsByMandantIdAndCategoryIdAndArrivalSpec(request.MandantId, category.Id, workDate), cancellationToken);

                    string sqlArrival = $"SELECT SUM(RoomAmount) FROM lnx.Reservation WHERE MandantId = {request.MandantId} AND CategoryId = {category.Id} AND (Arrival >= Convert(DATE, '{workDate}') AND Arrival < Convert(DATE, '{workDate.AddDays(1)}'))  AND ResKz <> 'S'";
                    vCat.Arrival = await _dapperRepository.QueryExecuteScalarAsync<int>(sqlArrival, cancellationToken);

                    // alle Reservations zählen die zur Category gehören und die am Tag abreisen
                    // vCat.Departure = await _reservationRepository.CountAsync(new ReservationsByMandantIdAndCategoryIdAndDepartureSpec(request.MandantId, category.Id, workDate), cancellationToken);

                    string sqlDeparture = $"SELECT SUM(RoomAmount) FROM lnx.Reservation WHERE MandantId = {request.MandantId} AND CategoryId = {category.Id} AND (Departure >= Convert(DATE, '{workDate}') AND Departure < Convert(DATE, '{workDate.AddDays(1)}')) AND ResKz <> 'S'";
                    vCat.Departure = await _dapperRepository.QueryExecuteScalarAsync<int>(sqlDeparture, cancellationToken);

                    // alle Reservations zählen die zur Category gehören und die am Tag bleiben
                    // vCat.Stay = await _reservationRepository.CountAsync(new ReservationsByMandantIdAndCategoryIdAndStaySpec(request.MandantId, category.Id, workDate), cancellationToken);

                    string sqlStay = $"SELECT SUM(RoomAmount) FROM lnx.Reservation WHERE MandantId = {request.MandantId} AND CategoryId = {category.Id} AND Arrival < Convert(DATE, '{workDate}') AND Departure >= Convert(DATE, '{workDate.AddDays(1)}') AND ResKz <> 'S'";
                    vCat.Stay = await _dapperRepository.QueryExecuteScalarAsync<int>(sqlStay, cancellationToken);

                    // alle Reservations die zur Category gehören und die an dem Tag anreise oder im Haus sind den PaxString laden
                    List<Reservation> reservationsArrival = await _reservationRepository.ListAsync(new ReservationsByMandantIdAndCategoryIdAndArrivalSpec(request.MandantId, category.Id, workDate), cancellationToken);

                    vCat.Adult = 0;
                    vCat.Child = 0;
                    vCat.Beds = 0;
                    vCat.ExtraBeds = 0;

                    // alle Reservations in reservationsArrival den PaxString laden und auswerten
                    foreach (Reservation r in reservationsArrival)
                    {
                        Pax pax = JsonSerializer.Deserialize<Pax>(r.PaxString);
                        vCat.Adult += (pax.Adult * (int)r.RoomAmount);
                        vCat.Child += (pax.Children.Count() * (int)r.RoomAmount);
                        int bedsAdult = pax.Adult;
                        if (bedsAdult <= category.NumberOfBeds)
                        {
                            vCat.Beds += bedsAdult * (int)r.RoomAmount;
                        }
                        else
                        {
                            vCat.Beds += (category.NumberOfBeds * (int)r.RoomAmount);
                            vCat.ExtraBeds += (bedsAdult - category.NumberOfBeds * (int)r.RoomAmount);
                        }

                        vCat.ExtraBeds += (pax.Children.Count(x => x.ExtraBed) * (int)r.RoomAmount);
                    }

                    await _repository.UpdateAsync(vCat, cancellationToken);

                    counter++;
                }
                else
                {
                    // neue VCat anlegen
                    VCat vCatNew = new VCat();
                    vCatNew.MandantId = request.MandantId;
                    vCatNew.Date = workDate;
                    vCatNew.CategoryId = category.Id;

                    // alle Rooms zählen die zur Category gehören
                    vCatNew.Amount = rooms.Where(x => x.CategoryId == category.Id).Count();

                    // alle Beds in Rooms zählen die zur Category gehören und in BedInventory eintragen
                    vCatNew.BedsInventory = rooms.Where(x => x.CategoryId == category.Id).Sum(x => x.Beds);

                    // alle BedsExtra in Rooms zählen die zur Category gehören und in ExtraBedsInventory eintragen
                    vCatNew.ExtraBedsInventory = rooms.Where(x => x.CategoryId == category.Id).Sum(x => x.BedsExtra);

                    // alle Rooms zählen die zur Category gehören und die am Tag Blocked sind
                    vCatNew.Blocked = rooms.Where(x => x.CategoryId == category.Id && x.Blocked && x.BlockedStart <= workDate && x.BlockedEnd > workDate).Count();

                    // alle Reservations zählen die zur Category gehören und die am Tag ankommen
                    // vCatNew.Arrival = await _reservationRepository.CountAsync(new ReservationsByMandantIdAndCategoryIdAndArrivalSpec(request.MandantId, category.Id, workDate), cancellationToken); // TODO * RoomAmount

                    string sqlArrival = $"SELECT SUM(RoomAmount) FROM lnx.Reservation WHERE MandantId = {request.MandantId} AND Arrival = '{workDate.Date}' AND ResKz<> 'S'";
                    vCatNew.Arrival = await _dapperRepository.QueryExecuteScalarAsync<int>(sqlArrival, cancellationToken);

                    // alle Reservations zählen die zur Category gehören und die am Tag abreisen
                    // vCatNew.Departure = await _reservationRepository.CountAsync(new ReservationsByMandantIdAndCategoryIdAndDepartureSpec(request.MandantId, category.Id, workDate), cancellationToken); // TODO * RoomAmount

                    string sqlDeparture = $"SELECT SUM(RoomAmount) FROM lnx.Reservation WHERE MandantId = {request.MandantId} AND CategoryId = {category.Id} AND Departure = Convert(DATE, '{workDate}') AND ResKz <> 'S'";
                    vCatNew.Departure = await _dapperRepository.QueryExecuteScalarAsync<int>(sqlDeparture, cancellationToken);

                    // alle Reservations zählen die zur Category gehören und die am Tag bleiben
                    // vCatNew.Stay = await _reservationRepository.CountAsync(new ReservationsByMandantIdAndCategoryIdAndStaySpec(request.MandantId, category.Id, workDate), cancellationToken); // TODO * RoomAmount

                    string sqlStay = $"SELECT SUM(RoomAmount) FROM lnx.Reservation WHERE MandantId = {request.MandantId} AND CategoryId = {category.Id} AND Arrival <= Convert(DATE, '{workDate}') AND Departure > Convert(DATE, '{workDate}') AND ResKz <> 'S'";
                    vCatNew.Stay = await _dapperRepository.QueryExecuteScalarAsync<int>(sqlStay, cancellationToken);


                    // alle Reservations die zur Category gehören und die an dem Tag ankommen oder im Haus sind den PaxString laden
                    List<Reservation> reservationsArrival = await _reservationRepository.ListAsync(new ReservationsByMandantIdAndCategoryIdAndArrivalSpec(request.MandantId, category.Id, workDate), cancellationToken);

                    // alle Reservations in reservationsArrival den PaxString laden und auswerten
                    foreach (Reservation r in reservationsArrival)
                    {
                        Pax pax = JsonSerializer.Deserialize<Pax>(r.PaxString);
                        vCatNew.Adult += (pax.Adult * (int)r.RoomAmount);
                        vCatNew.Child += (pax.Children.Count() * (int)r.RoomAmount);

                        int bedsAdult = pax.Adult;
                        if (bedsAdult <= category.NumberOfBeds)
                        {
                            vCatNew.Beds = bedsAdult * (int)r.RoomAmount;
                        }
                        else
                        {
                            vCatNew.Beds = category.NumberOfBeds * (int)r.RoomAmount;
                            vCatNew.ExtraBeds = bedsAdult - category.NumberOfBeds * (int)r.RoomAmount;
                        }

                        vCatNew.ExtraBeds += pax.Children.Count(x => x.ExtraBed) * (int)r.RoomAmount;
                    }


                    vCatNew = await _repository.AddAsync(vCatNew, cancellationToken);
                    counter++;
                }
            }
        }

        return counter;
    }
}

public class ReservationsByMandantIdAndCategoryIdAndStaySpec : Specification<Reservation>
{
    public ReservationsByMandantIdAndCategoryIdAndStaySpec(int mandantId, int categoryId, DateTime workDate)
    {
        Query.Where(x => x.MandantId == mandantId && x.CategoryId == categoryId && x.Arrival.Date < workDate.Date && x.Departure.Date > workDate.Date);
    }
}

public class ReservationsByMandantIdAndCategoryIdAndArrivalSpec : Specification<Reservation>
{
    public ReservationsByMandantIdAndCategoryIdAndArrivalSpec(int mandantId, int categoryId, DateTime workDate)
    {
        Query.Where(x => x.MandantId == mandantId && x.CategoryId == categoryId && x.Arrival.Date <= workDate.Date && x.Departure.Date > workDate.Date && x.ResKz != "S");
    }
}

public class ReservationsByMandantIdAndCategoryIdAndDepartureSpec : Specification<Reservation>
{
    public ReservationsByMandantIdAndCategoryIdAndDepartureSpec(int mandantId, int id, DateTime workDate)
    {
        Query.Where(x => x.MandantId == mandantId && x.CategoryId == id && x.Departure.Date == workDate.Date && x.ResKz != "S");
    }
}

public class MandantByIdSpec : Specification<Mandant>, ISingleResultSpecification
{
    public MandantByIdSpec(int mandantId)
    {
        Query.Where(x => x.Id == mandantId);
    }
}