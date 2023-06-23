using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Bookings;
public class SearchBookingByReservationIdRequest : IRequest<List<BookingDto>>
{
    public int MandantId { get; set; }
    public int ReservationId { get; set; }
}

public class SearchBookingByReservationIdRequestHandler : IRequestHandler<SearchBookingByReservationIdRequest, List<BookingDto>>
{
    private readonly IRepository<Booking> _repository;
    private readonly IStringLocalizer _t;

    public SearchBookingByReservationIdRequestHandler(IRepository<Booking> repository, IStringLocalizer<SearchBookingByReservationIdRequestHandler> localizer)
    {
        _repository = repository;
        _t = localizer;
    }

    public async Task<List<BookingDto>> Handle(SearchBookingByReservationIdRequest request, CancellationToken cancellationToken)
    {
        var bookings = await _repository.ListAsync(
            (ISpecification<Booking, BookingDto>)new BookingByReservationIdSpec(request.MandantId, request.ReservationId), cancellationToken);
        return bookings;
    }
}

public class BookingByReservationIdSpec : Specification<Booking, List<BookingDto>>
{
    public BookingByReservationIdSpec(int mandantId, int reservationId)
    {
        Query.Where(x => x.MandantId == mandantId && x.ReservationId == reservationId);
    }
}