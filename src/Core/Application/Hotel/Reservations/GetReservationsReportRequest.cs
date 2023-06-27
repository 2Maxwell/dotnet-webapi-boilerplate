using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationsReportRequest : IRequest<FileContentResult>
{
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public string? ResKz { get; set; } // TODO Prozedur für die einzelnen ResKz in der Abfrage erstellen NOTE wird durch Filter in der Liste ersetzt
}

public class ReservationsBySearchRequestArrivalDepartureSpec : Specification<Reservation, ReservationListDto>
{
    public ReservationsBySearchRequestArrivalDepartureSpec(GetReservationsReportRequest request)
    {
        Query
            .Include(x => x.Booker).ThenInclude(x => x.Salutation)
            .Include(x => x.Guest).ThenInclude(x => x.Salutation)
            .Include(x => x.Company)
            .Where(x => x.Arrival.Date == Convert.ToDateTime(request.Arrival).Date, request.Arrival != null)
            .Where(x => x.Departure.Date == Convert.ToDateTime(request.Departure).Date, request.Departure != null);
    }
}

public class GetReservationsReportRequestHandler : IRequestHandler<GetReservationsReportRequest, FileContentResult>
{
    private readonly IReadRepository<Reservation> _repository;
    private readonly IReadRepository<Person> _repositoryPerson;
    private readonly IReadRepository<Company> _repositoryCompany;
    private readonly IReadRepository<Salutation> _repositorySalutation;

    public GetReservationsReportRequestHandler(IReadRepository<Reservation> repository, IReadRepository<Person> repositoryPerson, IReadRepository<Company> repositoryCompany, IReadRepository<Salutation> repositorySalutation)
    {
        _repository = repository;
        _repositoryPerson = repositoryPerson;
        _repositoryCompany = repositoryCompany;
        _repositorySalutation = repositorySalutation;
    }

    public async Task<FileContentResult> Handle(GetReservationsReportRequest request, CancellationToken cancellationToken)
    {
        var spec = new ReservationsBySearchRequestArrivalDepartureSpec(request);
        var liste = await _repository.ListAsync(spec, cancellationToken: cancellationToken);

        // Prepare a Report: I will create the Report afterwards
        // Please Check if the ResultTyp is Correct now it's FileContentResult
        // Don't use your ServiceClasses just to get it correct in one place.
        // ReportName : Reservation1.frx


        // TODO: Correct this Result
        return new FileContentResult(Encoding.UTF8.GetBytes("Test"), "application/pdf");
    }
}