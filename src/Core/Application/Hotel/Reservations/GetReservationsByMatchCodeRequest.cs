using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationsByMatchCodeRequest : IRequest<List<ReservationListDto>>
{
    public int MandantId { get; set; }
    public string MatchCode { get; set; }
}

public class GetReservationsByMatchCodeRequestHandler : IRequestHandler<GetReservationsByMatchCodeRequest, List<ReservationListDto>>
{
    private readonly IReadRepository<Reservation> _repository;

    public GetReservationsByMatchCodeRequestHandler(IReadRepository<Reservation> repository)
    {
        _repository = repository;
    }

    public async Task<List<ReservationListDto>> Handle(GetReservationsByMatchCodeRequest request, CancellationToken cancellationToken)
    {
        var spec = new ReservationsByMatchCodeSpec(request);
        var liste = await _repository.ListAsync(spec, cancellationToken);
        return liste;
    }
}

public class ReservationsByMatchCodeSpec : Specification<Reservation, ReservationListDto>
{
    public ReservationsByMatchCodeSpec(GetReservationsByMatchCodeRequest request)
    {
        Query
            .Include(x => x.Booker).ThenInclude(x => x.Salutation)
            .Include(x => x.Guest).ThenInclude(x => x.Salutation)
            .Include(x => x.Company)
            .Where(x => x.MandantId == request.MandantId)
            .Where(x => x.MatchCode == request.MatchCode);
    }
}