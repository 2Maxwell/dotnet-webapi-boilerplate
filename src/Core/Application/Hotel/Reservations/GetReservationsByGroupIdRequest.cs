using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationsByGroupIdRequest : IRequest<List<ReservationListDto>>
{
    public int MandantId { get; set; }
    public int GroupId { get; set; }
}

public class GetReservationsByGroupIdRequestHandler : IRequestHandler<GetReservationsByGroupIdRequest, List<ReservationListDto>>
{
    private readonly IReadRepository<Reservation> _repository;
    private readonly IReadRepository<Person> _repositoryPerson;
    private readonly IReadRepository<Company> _repositoryCompany;
    private readonly IReadRepository<Salutation> _repositorySalutation;

    public GetReservationsByGroupIdRequestHandler(IReadRepository<Reservation> repository, IReadRepository<Person> repositoryPerson, IReadRepository<Company> repositoryCompany, IReadRepository<Salutation> repositorySalutation)
    {
        _repository = repository;
        _repositoryPerson = repositoryPerson;
        _repositoryCompany = repositoryCompany;
        _repositorySalutation = repositorySalutation;
    }

    public async Task<List<ReservationListDto>> Handle(GetReservationsByGroupIdRequest request, CancellationToken cancellationToken)
    {
        var spec = new ReservationsByGroupIdSpec(request);
        var liste = await _repository.ListAsync(spec, cancellationToken);
        return liste;
    }
}

public class ReservationsByGroupIdSpec : Specification<Reservation, ReservationListDto>
{
    public ReservationsByGroupIdSpec(GetReservationsByGroupIdRequest request)
    {
        Query
            .Include(x => x.Booker).ThenInclude(x => x.Salutation)
            .Include(x => x.Guest).ThenInclude(x => x.Salutation)
            .Include(x => x.Company)
            .Where(x => x.MandantId == request.MandantId)
            .Where(x => x.GroupMasterId == request.GroupId);
    }
}
