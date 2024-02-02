using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class SearchReservationsRequest : PaginationFilter, IRequest<PaginationResponse<ReservationListDto>>
{

    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public string? ResKz { get; set; } // TODO Prozedur für die einzelnen ResKz in der Abfrage erstellen NOTE wird durch Filter in der Liste ersetzt
    public string? BookerName { get; set; }
    public string? GuestName { get; set; } // wird nicht mehr benötigt
    public string? CompanyName { get; set; }
}

public class ReservationsBySearchRequestArrivalGuestNameCompanyNameSpec : EntitiesByPaginationFilterSpec<Reservation, ReservationListDto>
{
    public ReservationsBySearchRequestArrivalGuestNameCompanyNameSpec(SearchReservationsRequest request)
    : base(request)
    {
        if (request.ResKz is not null)
        {
            string[] resKz = request.ResKz!.Split(',');
            Query
                .Include(x => x.Booker).ThenInclude(x => x.Salutation)
                .Include(x => x.Guest).ThenInclude(x => x.Salutation)
                .Include(x => x.Company)
                .Where(x => x.MandantId == request.MandantId)
                .Where(x => x.Booker.Name!.StartsWith(request.BookerName!) || x.Guest.Name!.StartsWith(request.BookerName!), request.BookerName != null || request.GuestName != null)
                .Where(x => x.Company!.Name1!.StartsWith(request.CompanyName!), request.CompanyName != null)
                .Where(x => resKz.Contains(x.ResKz), request.ResKz != null)
                .Where(x => x.Arrival.Date == Convert.ToDateTime(request.Arrival).Date, request.Arrival != null)
                .Where(x => x.Departure.Date >= Convert.ToDateTime(request.Departure).Date & x.Departure.Date < Convert.ToDateTime(request.Departure).AddDays(1).Date, request.Departure != null);
        }
        else
        {
            Query
                .Include(x => x.Booker).ThenInclude(x => x.Salutation)
                .Include(x => x.Guest).ThenInclude(x => x.Salutation)
                .Include(x => x.Company)
                .Where(x => x.MandantId == request.MandantId)
                .Where(x => x.Booker.Name!.StartsWith(request.BookerName!) || x.Guest.Name!.StartsWith(request.BookerName!), request.BookerName != null || request.GuestName != null)
                .Where(x => x.Company!.Name1!.StartsWith(request.CompanyName!), request.CompanyName != null)
                .Where(x => x.Arrival.Date == Convert.ToDateTime(request.Arrival).Date, request.Arrival != null)
                .Where(x => x.Departure.Date >= Convert.ToDateTime(request.Departure).Date & x.Departure.Date < Convert.ToDateTime(request.Departure).AddDays(1).Date, request.Departure != null);

            // .Where(x => x.Departure.Date == Convert.ToDateTime(request.Departure).Date, request.Departure != null);
        }
    }
}

public class SearchReservationsRequestHandler : IRequestHandler<SearchReservationsRequest, PaginationResponse<ReservationListDto>>
{
    private readonly IReadRepository<Reservation> _repository;
    private readonly IReadRepository<Person> _repositoryPerson;
    private readonly IReadRepository<Company> _repositoryCompany;
    private readonly IReadRepository<Salutation> _repositorySalutation;

    public SearchReservationsRequestHandler(IReadRepository<Reservation> repository, IReadRepository<Person> repositoryPerson, IReadRepository<Company> repositoryCompany, IReadRepository<Salutation> repositorySalutation)
    {
        _repository = repository;
        _repositoryPerson = repositoryPerson;
        _repositoryCompany = repositoryCompany;
        _repositorySalutation = repositorySalutation;
    }

    public async Task<PaginationResponse<ReservationListDto>> Handle(SearchReservationsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ReservationsBySearchRequestArrivalGuestNameCompanyNameSpec(request);
        var liste = await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
        // liste.PageSize = 50;
        Console.WriteLine($"Anzahl Reservierungen gefunden: {liste.TotalCount}");
        return liste;
    }
}