using FSH.WebApi.Application.Hotel.Periods;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Packages;

public class GetPackageRequest : IRequest<PackageDto>
{
    public int Id { get; set; }
    public GetPackageRequest(int id) => Id = id;
}

public class GetPackageRequestHandler : IRequestHandler<GetPackageRequest, PackageDto>
{

    // private readonly HttpClient _httpClient;
    private readonly IRepository<Package> _repository;
    private readonly IStringLocalizer<GetPackageRequestHandler> _localizer;
    private readonly IRepository<PackageItem> _packageItemRepository;

    public GetPackageRequestHandler(IRepository<Package> repository, IRepository<PackageItem> packageItemRepository, IStringLocalizer<GetPackageRequestHandler> localizer) =>
        (_repository, _packageItemRepository, _localizer) = (repository, packageItemRepository, localizer);

    public async Task<PackageDto> Handle(GetPackageRequest request, CancellationToken cancellationToken)
    {
        PackageDto? packageDto = await _repository.GetBySpecAsync(
            (ISpecification<Package, PackageDto>)new PackageByIdSpec(request.Id), cancellationToken);

        // ?? throw new NotFoundException(string.Format(_localizer["package.notfound"], request.Id));

        if (packageDto == null) throw new NotFoundException(string.Format(_localizer["package.notfound"], request.Id));

        var listPackageItems = await _packageItemRepository.ListAsync(
            (ISpecification<PackageItem, PackageItemDto>)new PackageItemByPackageIdSpec(request.Id), cancellationToken);

        packageDto.PackageItems = listPackageItems;

        return packageDto;
    }
}

public class PackageByIdSpec : Specification<Package, PackageDto>, ISingleResultSpecification
{
    public PackageByIdSpec(int id) => Query.Where(x => x.Id == id);
}

public class PeriodByPackageSourceSpec : Specification<Period, PeriodDto>
{
    public PeriodByPackageSourceSpec(int id) =>
        Query.Where(x => x.SourceId == id && x.Source == "package")
             .OrderBy(x => x.Start);
}