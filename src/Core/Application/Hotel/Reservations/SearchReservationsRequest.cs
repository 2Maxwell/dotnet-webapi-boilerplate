using FSH.WebApi.Application.Environment.Companys;
using FSH.WebApi.Application.Environment.Persons;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;
using Mapster;

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
    : base(request) =>
        Query
            .Include(x => x.Booker).ThenInclude(x => x.Salutation)
            .Include(x => x.Guest).ThenInclude(x => x.Salutation)
            .Include(x => x.Company)
            .Where(x => x.MandantId == request.MandantId)
            //.Include(x => x.TravelAgent)
            .Where(x => x.Booker.Name!.StartsWith(request.BookerName!) || x.Guest.Name!.StartsWith(request.BookerName!), request.BookerName != null || request.GuestName != null)
            //.Where(x => x.Guest.Name!.StartsWith(request.GuestName!), request.GuestName != null)
            .Where(x => x.Company!.Name1!.StartsWith(request.CompanyName!), request.CompanyName != null)
            .Where(x => x.Arrival.Date == Convert.ToDateTime(request.Arrival).Date, request.Arrival != null)
            .Where(x => x.Departure.Date == Convert.ToDateTime(request.Departure).Date, request.Departure != null);

    //.Where(x => DateOnly.FromDateTime(x.Departure) == DateOnly.FromDateTime(Convert.ToDateTime(request.Departure)), request.Departure != null);
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
        //foreach (var item in liste.Data)
        //{
        //    if (item.BookerId != null)
        //    {
        //        var personDto = await _repositoryPerson.GetByIdAsync(item.BookerId, cancellationToken);
        //        personDto.Salutation = await _repositorySalutation.GetByIdAsync(personDto.SalutationId, cancellationToken);
        //        item.BookerDto = new PersonDto();
        //        item.BookerDto = personDto.Adapt<PersonDto>();
        //    }

        //    if (item.GuestId != null)
        //    {
        //        var personDto = await _repositoryPerson.GetByIdAsync(item.GuestId, cancellationToken); //.Adapt<PersonDto>();
        //        personDto.Salutation = await _repositorySalutation.GetByIdAsync(personDto.SalutationId, cancellationToken);
        //        item.GuestDto = personDto.Adapt<PersonDto>();
        //    }

        //    if (item.CompanyId != null)
        //    {
        //        var companyDto = (await _repositoryCompany.GetByIdAsync(item.CompanyId, cancellationToken)).Adapt<CompanyDto>();
        //        item.CompanyDto = companyDto;
        //    }

        //    if (item.TravelAgentId != null)
        //    {
        //        var companyDto = (await _repositoryCompany.GetByIdAsync(item.TravelAgentId, cancellationToken)).Adapt<CompanyDto>();
        //        item.TravelAgentDto = companyDto;
        //    }
        //}

        return liste;
    }
}