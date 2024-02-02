using FSH.WebApi.Application.Environment.Pictures;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;
using System.Linq.Expressions;

namespace FSH.WebApi.Application.Hotel.Packages;
public class GetPackageExtendsRequest : IRequest<List<PackageExtendDto>>
{
    public GetPackageExtendsRequest(int mandantId, PackageTargetEnum packageTargetEnum, DateTime startDate, DateTime endDate)
    {
        MandantId = mandantId;
        this.packageTargetEnum = packageTargetEnum;
        StartDate = startDate;
        EndDate = endDate;
    }

    public int MandantId { get; set; }
    public PackageTargetEnum packageTargetEnum { get; set; } // kann 0 sein, dann werden alle übertragen
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}

public class PackageByMandantIdAndPackageTargetSpec : Specification<Package, PackageDto>
{
    public PackageByMandantIdAndPackageTargetSpec(int mandantId, PackageTargetEnum packageTargetEnum)
    {
        if (packageTargetEnum == 0)
        {
            Query.Where(c => c.MandantId == mandantId);
        }
        else
        {
            Query.Where(c => c.MandantId == mandantId && c.PackageTargetEnum.Contains(Enum.GetName(typeof(PackageTargetEnum), packageTargetEnum)));
        }

    }
}

public class GetPackageExtendsRequestHandler : IRequestHandler<GetPackageExtendsRequest, List<PackageExtendDto>>
{
    private readonly IRepository<Package> _repository;
    private readonly IReadRepository<PackageItem> _repositoryPackageItem;
    private readonly IStringLocalizer<GetPackageExtendsRequestHandler> _localizer;
    private readonly IReadRepository<Picture> _repositoryPicture;

    public GetPackageExtendsRequestHandler(IRepository<Package> repository, IReadRepository<PackageItem> repositoryPackageItem, IStringLocalizer<GetPackageExtendsRequestHandler> localizer, IReadRepository<Picture> repositoryPicture)
    {
        _repository = repository;
        _repositoryPackageItem = repositoryPackageItem;
        _localizer = localizer;
        _repositoryPicture = repositoryPicture;
    }

    public async Task<List<PackageExtendDto>> Handle(GetPackageExtendsRequest request, CancellationToken cancellationToken)
    {
        var listPackage = await _repository.ListAsync((ISpecification<Package, PackageDto>)new PackageByMandantIdAndPackageTargetSpec(request.MandantId, request.packageTargetEnum), cancellationToken);

        if (listPackage is null) throw new NotFoundException(string.Format(_localizer["PackageExtends.notfound"], request.MandantId));

        List<PackageExtendDto> listPackageExtended = new();

        foreach (var pack in listPackage)
        {
            // lade PackageItemDto
            List<PackageItemDto> listPackageItems = new List<PackageItemDto>();
            pack.PackageItems = await _repositoryPackageItem.ListAsync((ISpecification<PackageItem, PackageItemDto>)new PackageItemByPackageIdSpec(pack.Id), cancellationToken);

            PackageExtendDto packageExtendDto = new PackageExtendDto();
            packageExtendDto.PackageDto = pack;

            var specImagePath = new PictureByMatchCodeOrderedSpec(pack.Kz!.Trim(), request.MandantId);
            var pic = await _repositoryPicture.GetBySpecAsync(specImagePath, cancellationToken);
            packageExtendDto.ImagePath = pic != null ? pic.ImagePath : string.Empty;

            packageExtendDto.Amount = 0;
            packageExtendDto.Price = pack.PackageItems.Where(x => x.Start <= request.StartDate && x.End >= request.EndDate).Sum(x => x.Price);

            if (pack.PackageBookingRhythmEnumId == (int)PackageBookingRhythmEnum.perAppointment)
            {
                packageExtendDto.AppointmentTargetEnum = (int)pack.AppointmentTargetEnum!;
                Console.WriteLine("AppointmentTargetEnum: " + packageExtendDto.AppointmentTargetEnum);
            }

            //if (pack.PackageTargetEnum.Contains("Restaurant")) packageExtendDto.AppointmentTargetEnum = (int)AppointmentTargetEnum.Restaurant;
            //if (pack.PackageTargetEnum.Contains("Wellness")) packageExtendDto.AppointmentTargetEnum = (int)AppointmentTargetEnum.Wellness;
            //if (pack.PackageTargetEnum.Contains("Bike")) packageExtendDto.AppointmentTargetEnum = (int)AppointmentTargetEnum.BikeRental;

            listPackageExtended.Add(packageExtendDto);
        }

        return listPackageExtended;

    }

}
