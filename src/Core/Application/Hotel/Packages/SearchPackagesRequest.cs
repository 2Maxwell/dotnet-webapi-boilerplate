using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Packages;

public class SearchPackagesRequest : PaginationFilter, IRequest<PaginationResponse<PackageDto>>
{
}

public class PackagesBySearchRequestSpec : EntitiesByPaginationFilterSpec<Package, PackageDto>
{
    public PackagesBySearchRequestSpec(SearchPackagesRequest request)
        : base(request) =>
        Query
        .OrderBy(p => p.Name, !request.HasOrderBy());
}

public class PackageItemByPackageIdSpec : Specification<PackageItem, PackageItemDto>
{
    public PackageItemByPackageIdSpec(int id) =>
        Query.Where(x => x.PackageId == id)
             .OrderBy(x => x.Start)
             .ThenBy(x => x.End)
             .ThenBy(x => x.ItemId);
}

public class SearchPackagesRequestHandler : IRequestHandler<SearchPackagesRequest, PaginationResponse<PackageDto>>
{
    private readonly IReadRepository<Package> _repository;
    private readonly IRepository<PackageItem> _packageItemRepository;

    public SearchPackagesRequestHandler(IReadRepository<Package> repository, IRepository<PackageItem> packageItemRepository) =>
        (_repository, _packageItemRepository) = (repository, packageItemRepository);

    public async Task<PaginationResponse<PackageDto>> Handle(SearchPackagesRequest request, CancellationToken cancellationToken)
    {
        var spec = new PackagesBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        foreach (PackageDto package in list)
        {
            List<PackageItemDto> listPackageItems = new List<PackageItemDto>();
            listPackageItems = await _packageItemRepository.ListAsync((ISpecification<PackageItem, PackageItemDto>)new PackageItemByPackageIdSpec(package.Id), cancellationToken);

            package.PackageItems = listPackageItems;
        }

        return new PaginationResponse<PackageDto>(list, count, request.PageNumber, request.PageSize);
    }
}