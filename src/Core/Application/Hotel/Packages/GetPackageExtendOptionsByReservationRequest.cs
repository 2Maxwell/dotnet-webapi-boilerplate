using FSH.WebApi.Application.General.Appointments;
using FSH.WebApi.Domain.General;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using Mapster;

namespace FSH.WebApi.Application.Hotel.Packages;
public class GetPackageExtendOptionsByReservationRequest : IRequest<List<PackageExtendDto>>
{
    public GetPackageExtendOptionsByReservationRequest(int mandantId, int reservationId)
    {
        MandantId = mandantId;
        ReservationId = reservationId;
    }

    public int MandantId { get; set; }
    public int ReservationId { get; set; }
}

public class PackageExtendOptionsByReservationSpec : Specification<PackageExtend, PackageExtendDto>
{
    public PackageExtendOptionsByReservationSpec(int mandantId, int reservationId) =>
        Query
        .Where(x => x.SourceId == reservationId && x.MandantId == mandantId);
}

public class GetPackageExtendOptionsByReservationRequestHandler : IRequestHandler<GetPackageExtendOptionsByReservationRequest, List<PackageExtendDto>>
{
    private readonly IRepository<PackageExtend> _repository;
    private readonly IRepository<Package> _packageRepository;
    private readonly IRepository<PackageItem> _packageItemRepository;
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IStringLocalizer<GetPackageExtendOptionsByReservationRequestHandler> _localizer;

    public GetPackageExtendOptionsByReservationRequestHandler(IRepository<PackageExtend> repository, IRepository<Package> packageRepository, IRepository<PackageItem> packageItemRepository, IRepository<Appointment> appointmentRepository, IStringLocalizer<GetPackageExtendOptionsByReservationRequestHandler> localizer)
    {
        _repository = repository;
        _packageRepository = packageRepository;
        _packageItemRepository = packageItemRepository;
        _appointmentRepository = appointmentRepository;
        _localizer = localizer;
    }

    public async Task<List<PackageExtendDto>> Handle(GetPackageExtendOptionsByReservationRequest request, CancellationToken cancellationToken)
    {
        List<PackageExtendDto> list = new();
        list = await _repository.ListAsync((ISpecification<PackageExtend, PackageExtendDto>)new PackageExtendOptionsByReservationSpec(request.MandantId, request.ReservationId), cancellationToken);
        Console.WriteLine("Liste Count: " + (list == null ? 0 : list.Count.ToString()));

        if(list.Count > 0 )
        {
            foreach (var item in list)
            {
                var packageSpec = new PackageByIdSpec(item.PackageId);
                item.PackageDto = (await _packageRepository.GetBySpecAsync(packageSpec, cancellationToken)).Adapt<PackageDto>();

                var packageItemSpec = new PackageItemByPackageIdSpec(item.PackageId);
                item.PackageDto.PackageItems = (await _packageItemRepository.ListAsync(packageItemSpec, cancellationToken)).Adapt<List<PackageItemDto>>();

                if(item.AppointmentId.HasValue && item.AppointmentId > 0)
                {
                    item.AppointmentDto = (await _appointmentRepository.GetByIdAsync(item.AppointmentId.Value, cancellationToken)).Adapt<AppointmentDto>();
                }
            }
        }

        if (list == null) throw new NotFoundException(string.Format(_localizer["packages.notfound"], request.ReservationId));

        return list;
    }
}
