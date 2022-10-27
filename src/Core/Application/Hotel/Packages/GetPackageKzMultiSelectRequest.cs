using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Packages;

public class GetPackageKzMultiSelectRequest : IRequest<List<PackageKzMultiSelectDto>>
{
    public int MandantId { get; set; }
    public GetPackageKzMultiSelectRequest(int mandantId) => MandantId = mandantId;
}

public class PackageKzByMandantIdSpec : Specification<Package, PackageKzMultiSelectDto>
{
    public PackageKzByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId);
    }
}

public class GetPackageKzMultiSelectRequestHandler : IRequestHandler<GetPackageKzMultiSelectRequest, List<PackageKzMultiSelectDto>>
{
    private readonly IRepository<Package> _repository;
    private readonly IStringLocalizer<GetPackageKzMultiSelectRequestHandler> _localizer;

    public GetPackageKzMultiSelectRequestHandler(IRepository<Package> repository, IStringLocalizer<GetPackageKzMultiSelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);
    public async Task<List<PackageKzMultiSelectDto>> Handle(GetPackageKzMultiSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Package, PackageKzMultiSelectDto>)new PackageKzByMandantIdSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["PackagesKz.notfound"], request.MandantId));
}
