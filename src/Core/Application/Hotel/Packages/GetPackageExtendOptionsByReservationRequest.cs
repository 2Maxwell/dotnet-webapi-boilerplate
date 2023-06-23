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
    private readonly IStringLocalizer<GetPackageExtendOptionsByReservationRequestHandler> _localizer;

    public GetPackageExtendOptionsByReservationRequestHandler(IRepository<PackageExtend> repository, IRepository<Package> packagerepository, IRepository<PackageItem> packageItemRepository, IStringLocalizer<GetPackageExtendOptionsByReservationRequestHandler> localizer)
    {
        _repository = repository;
        _packageRepository = packagerepository;
        _packageItemRepository = packageItemRepository;
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
            }
        }

        if (list == null) throw new NotFoundException(string.Format(_localizer["reservation.notfound"], request.ReservationId));

        return list;
    }
}
