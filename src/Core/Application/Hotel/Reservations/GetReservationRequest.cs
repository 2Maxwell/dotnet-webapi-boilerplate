using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceTags;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using Mapster;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationRequest : IRequest<ReservationDto>
{
    public GetReservationRequest(int id, int mandantId)
    {
        Id = id;
        MandantId = mandantId;
    }

    public int Id { get; set; }
    public int MandantId { get; set; }
}

public class ReservationByIdSpec : Specification<Reservation, ReservationDto>, ISingleResultSpecification
{
    public ReservationByIdSpec(GetReservationRequest request) => Query
        .Where(x => x.MandantId == request.MandantId && x.Id == request.Id)
        .Include(x => x.Booker);
        //.ThenInclude(x => x.Salutation);
}

public class PriceTagByReservationIdSpec : Specification<PriceTag, PriceTagDto>, ISingleResultSpecification
{
    public PriceTagByReservationIdSpec(int id) => Query
        .Where(x => x.ReservationId == id)
        .Include(x => x.PriceTagDetails);
}

public class GetReservationRequestHandler : IRequestHandler<GetReservationRequest, ReservationDto>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IRepository<PriceTag> _repositoryPriceTag;
    private readonly IRepository<PackageExtend> _repositoryPackageExtend;
    private readonly IRepository<Package> _repositoryPackage;
    private readonly IRepository<PackageItem> _repositoryPackageItem;
    private readonly IStringLocalizer<GetReservationRequestHandler> _localizer;

    public GetReservationRequestHandler(IRepository<Reservation> repository, IRepository<PriceTag> repositoryPriceTag, IRepository<PackageExtend> repositoryPackageExtend, IRepository<Package> repositoryPackage, IRepository<PackageItem> repositoryPackageItem, IStringLocalizer<GetReservationRequestHandler> localizer)
    {
        _repository = repository;
        _repositoryPriceTag = repositoryPriceTag;
        _repositoryPackageExtend = repositoryPackageExtend;
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _localizer = localizer;
    }

    public async Task<ReservationDto> Handle(GetReservationRequest request, CancellationToken cancellationToken)
    {
        //    ReservationDto? reservationDto = await _repository.GetBySpecAsync(
        //(ISpecification<Reservation, ReservationDto>)new ReservationByIdSpec(request.Id), cancellationToken);

        //    if (reservationDto == null) throw new NotFoundException(string.Format(_localizer["reservation.notfound"], request.Id));

        ReservationDto? reservationDto = await _repository.GetBySpecAsync(
(ISpecification<Reservation, ReservationDto>)new ReservationByIdSpec(request), cancellationToken)
            ?? throw new NotFoundException(string.Format(_localizer["Hallo reservation {0} NotFound"], request.Id));

        reservationDto.PriceTagDto = await _repositoryPriceTag.GetBySpecAsync(
             (ISpecification<PriceTag, PriceTagDto>)new PriceTagByReservationIdSpec(request.Id), cancellationToken);

        reservationDto.PackageExtendOptionDtos = await _repositoryPackageExtend.ListAsync(
            (ISpecification<PackageExtend, PackageExtendDto>)new PackageExtendOptionsByReservationSpec(reservationDto.MandantId, request.Id), cancellationToken);

        if(reservationDto.PackageExtendOptionDtos.Count > 0)
        {
            foreach (var item in reservationDto.PackageExtendOptionDtos)
            {
                var packageSpec = new PackageByIdSpec(item.PackageId);
                item.PackageDto = (await _repositoryPackage.GetBySpecAsync(packageSpec, cancellationToken)).Adapt<PackageDto>();

                var packageItemSpec = new PackageItemByPackageIdSpec(item.PackageId);
                item.PackageDto.PackageItems = (await _repositoryPackageItem.ListAsync(packageItemSpec, cancellationToken)).Adapt<List<PackageItemDto>>();
            }
        }

        return reservationDto;
    }
}