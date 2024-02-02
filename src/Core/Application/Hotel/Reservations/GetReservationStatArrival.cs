using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;
using Newtonsoft.Json;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationStatArrival : IRequest<ReservationStatArrivalDto>
{
    public GetReservationStatArrival(int mandantId, DateTime? date)
    {
        MandantId = mandantId;
        Date = date;
    }

    public int MandantId { get; set; }
    public DateTime? Date { get; set; }
}

public class GetReservationStatArrivalHandler : IRequestHandler<GetReservationStatArrival, ReservationStatArrivalDto>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IStringLocalizer<GetReservationStatArrivalHandler> _localizer;

    public GetReservationStatArrivalHandler(IRepository<Reservation> repository, IStringLocalizer<GetReservationStatArrivalHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<ReservationStatArrivalDto> Handle(GetReservationStatArrival request, CancellationToken cancellationToken)
    {
        var reservations = (await _repository.ListAsync(cancellationToken))
            .Where(x => x.MandantId == request.MandantId
                && x.Arrival.Date == Convert.ToDateTime(request.Date).Date
                && x.ResKz == "P" | x.ResKz == "R" | x.ResKz == "C").ToList();

        var reservationStatArrivalDto = new ReservationStatArrivalDto
        {
            MandantId = request.MandantId,
            Date = request.Date,
            ReservationToday = reservations.Count(),
            ReservationRoomsToday = (int?)reservations.Sum(x => x.RoomAmount),
            ReservationAdultToday = 0,
            ReservationChildToday = 0,
            ReservationExtraBedToday = 0, // result.Sum(x => x.PersonShopItems.Where(x => x.AgeGroup == AgeGroup.ExtraBed).Count()),
            ReservationTodayDone = reservations.Where(x => x.ResKz == "C" || x.ResKz == "O").Count(),
            ReservationRoomsTodayDone = (int)reservations.Where(x => x.ResKz == "C" || x.ResKz == "O").Sum(x => x.RoomAmount),
            ReservationAdultTodayDone = 0, //result.Where(x => x.ResKz == "C" || x.ResKz == "O").Sum(x => x.Persons),
            ReservationChildTodayDone = 0, //result.Where(x => x.ResK)
            ReservationExtraBedTodayDone = 0 
        };

        foreach (Reservation res in reservations)
        {
            Pax pax = JsonConvert.DeserializeObject<Pax>(res.PaxString ?? string.Empty);
            if (pax != null)
            {
                if (res.ResKz == "C" || res.ResKz == "O")
                {
                    reservationStatArrivalDto.ReservationAdultTodayDone += pax.Adult;
                    reservationStatArrivalDto.ReservationChildTodayDone += pax.Children!.Count;
                    reservationStatArrivalDto.ReservationExtraBedTodayDone += pax.Children!.Where(x => x.ExtraBed).Count();
                }
                else
                {
                    reservationStatArrivalDto.ReservationAdultToday += pax.Adult;
                    reservationStatArrivalDto.ReservationChildToday += pax.Children!.Count;
                    reservationStatArrivalDto.ReservationExtraBedToday += pax.Children!.Where(x => x.ExtraBed).Count();
                }
            }
        }

        return reservationStatArrivalDto;
    }
}
