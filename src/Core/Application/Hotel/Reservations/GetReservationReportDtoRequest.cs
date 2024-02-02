using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;
using System.Text.Json;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationReportDtoRequest : IRequest<List<ReservationReportDto>>
{
    public int MandantId { get; set; }
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public string? ResKz { get; set; }
    public string? ResponseType { get; set; }
}

public class ReservationsBySearchRequestArrivalDepartureReportSpec : Specification<Reservation, ReservationReportDto>
{
    public ReservationsBySearchRequestArrivalDepartureReportSpec(GetReservationReportDtoRequest request)
    {
        Query
            .Include(x => x.Booker).ThenInclude(x => x.Salutation)
            .Include(x => x.Guest).ThenInclude(x => x.Salutation)
            .Include(x => x.Company)
            .Where(x => x.MandantId == request.MandantId)
            .Where(x => x.Arrival.Date == Convert.ToDateTime(request.Arrival).Date, request.Arrival != null)
            .Where(x => x.Departure.Date == Convert.ToDateTime(request.Departure).Date, request.Departure != null)
            .Where(x => x.ResKz == request.ResKz, request.ResKz != null);
    }
}

public class GetReservationReportDtoRequestHandler : IRequestHandler<GetReservationReportDtoRequest, List<ReservationReportDto>>
{
    private readonly IDapperRepository _repositoryDapper;
    private readonly IReadRepository<Reservation> _repository;
    private readonly IReadRepository<Category> _repositoryCategory;
    private readonly IReadRepository<BookingPolicy> _repositoryBookingPolicy;
    private readonly IReadRepository<CancellationPolicy> _repositoryCancellationPolicy;

    public GetReservationReportDtoRequestHandler(IDapperRepository repositoryDapper, IReadRepository<Reservation> repository, IReadRepository<Category> repositoryCategory, IReadRepository<BookingPolicy> repositoryBookingPolicy, IReadRepository<CancellationPolicy> repositoryCancellationPolicy)
    {
        _repositoryDapper = repositoryDapper;
        _repository = repository;
        _repositoryCategory = repositoryCategory;
        _repositoryBookingPolicy = repositoryBookingPolicy;
        _repositoryCancellationPolicy = repositoryCancellationPolicy;
    }

    public async Task<List<ReservationReportDto>> Handle(GetReservationReportDtoRequest request, CancellationToken cancellationToken)
    {
        var spec = new ReservationsBySearchRequestArrivalDepartureReportSpec(request);
        var liste = await _repository.ListAsync(spec, cancellationToken: cancellationToken);

        foreach (ReservationReportDto item in liste)
        {
            var cancellationPolicy = await _repositoryCancellationPolicy.GetByIdAsync(item.CancellationPolicyId, cancellationToken);
            item.CancellationPolicyName = cancellationPolicy!.Kz;

            var bookingPolicy = await _repositoryBookingPolicy.GetByIdAsync(item.BookingPolicyId, cancellationToken);
            item.BookingPolicyName = bookingPolicy!.Kz;

            item.CategoryName = await _repositoryDapper.QueryExecuteScalarAsync<string>(
                $"SELECT Kz FROM lnx.Category WHERE Id = {item.CategoryId}", cancellationToken);

            string paxStringSource = item.PaxString;
            Pax pax = JsonSerializer.Deserialize<Pax>(paxStringSource);
            string paxOutput = pax.Adult.ToString() + "|" + pax.Children.Count.ToString() + "|" + pax.Beds.ToString();
            item.PaxString = paxOutput;
        }

        return liste;
    }
}