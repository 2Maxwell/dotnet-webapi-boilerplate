using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceTags;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.General;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationsByResKzRequest : IRequest<List<ReservationDto>>
{
    public GetReservationsByResKzRequest(int mandantId, string resKz, DateTime? arrival, DateTime? departure)
    {
        MandantId = mandantId;
        ResKz = resKz;
        Arrival = arrival;
        Departure = departure;
    }

    public int MandantId { get; set; }
    public string ResKz { get; set; }
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
}

public class GetReservationsByResKzRequestHandler : IRequestHandler<GetReservationsByResKzRequest, List<ReservationDto>>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IRepository<PriceTag> _repositoryPriceTag;
    private readonly IRepository<PackageExtend> _repositoryPackageExtend;
    private readonly IRepository<Package> _repositoryPackage;
    private readonly IRepository<PackageItem> _repositoryPackageItem;
    private readonly IRepository<Booking> _repositoryBooking;
    private readonly IRepository<Appointment> _repositoryAppointment;

    public GetReservationsByResKzRequestHandler(IRepository<Reservation> repository, IRepository<PriceTag> repositoryPriceTag, IRepository<PackageExtend> repositoryPackageExtend, IRepository<Package> repositoryPackage, IRepository<PackageItem> repositoryPackageItem, IRepository<Booking> repositoryBooking, IRepository<Appointment> repositoryAppointment)
    {
        _repository = repository;
        _repositoryPriceTag = repositoryPriceTag;
        _repositoryPackageExtend = repositoryPackageExtend;
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _repositoryBooking = repositoryBooking;
        _repositoryAppointment = repositoryAppointment;
    }

    public async Task<List<ReservationDto>> Handle(GetReservationsByResKzRequest request, CancellationToken cancellationToken)
    {
        var spec = new ReservationsByResKzSpec(request);
        var liste = await _repository.ListAsync(spec, cancellationToken);

        foreach (ReservationDto res in liste)
        {
            res.PriceTagDto = await _repositoryPriceTag.GetBySpecAsync(
                 (ISpecification<PriceTag, PriceTagDto>)new PriceTagByReservationIdSpec(res.Id), cancellationToken);

            // PackageExtend mit allen PackageItems und Package und Appointment wenn vorhanden
            GetPackageExtendOptionsByReservationRequest getPackageExtendOptionsByReservationRequest = new GetPackageExtendOptionsByReservationRequest(res.MandantId, res.Id);
            PackageExtendOptionsByReservationSpec getPackageExtendOptionsByReservationSpec = new PackageExtendOptionsByReservationSpec(getPackageExtendOptionsByReservationRequest.MandantId, getPackageExtendOptionsByReservationRequest.ReservationId);
            IStringLocalizer<GetPackageExtendOptionsByReservationRequestHandler> _localizerAppointment = null;

            GetPackageExtendOptionsByReservationRequestHandler getPackageExtendOptionsByReservationRequestHandler = new GetPackageExtendOptionsByReservationRequestHandler(_repositoryPackageExtend, _repositoryPackage, _repositoryPackageItem, _repositoryAppointment, _localizerAppointment);
            res.PackageExtendOptionDtos = await getPackageExtendOptionsByReservationRequestHandler.Handle(getPackageExtendOptionsByReservationRequest, cancellationToken);
        }

        return liste;
    }
}

public class ReservationsByResKzSpec : Specification<Reservation, ReservationDto>
{
    public ReservationsByResKzSpec(GetReservationsByResKzRequest request)
    {
        Query
            .Include(x => x.Booker).ThenInclude(x => x.Salutation)
            .Include(x => x.Guest).ThenInclude(x => x.Salutation)
            .Include(x => x.Company)
            .Where(x => x.MandantId == request.MandantId)
            .Where(x => x.ResKz == request.ResKz)
            .Where(x => x.Arrival >= request.Arrival!.Value.Date, request.Arrival.HasValue)
            .Where(x => x.Departure <= request.Departure!.Value.Date, request.Departure.HasValue);
    }
}