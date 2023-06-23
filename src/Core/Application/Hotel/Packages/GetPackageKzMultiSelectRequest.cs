using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Packages;

public class GetPackageKzMultiSelectRequest : IRequest<List<PackageKzMultiSelectDto>>
{
    public GetPackageKzMultiSelectRequest(int mandantId, PackageTargetEnum packageTargetEnum)
    {
        MandantId = mandantId;
        this.packageTargetEnum = packageTargetEnum;
    }

    public int MandantId { get; set; }
    public PackageTargetEnum packageTargetEnum { get; set; }
}

public class PackageKzByMandantIdAndPackageTargetSpec : Specification<Package, PackageKzMultiSelectDto>
{
    public PackageKzByMandantIdAndPackageTargetSpec(int mandantId, PackageTargetEnum packageTargetEnum)
    {
        string test = Enum.GetName(typeof(PackageTargetEnum), packageTargetEnum);
        Query.Where(c => c.MandantId == mandantId && c.PackageTargetEnum.Contains(Enum.GetName(typeof(PackageTargetEnum), packageTargetEnum)));
    }
}

public class GetPackageKzMultiSelectRequestHandler : IRequestHandler<GetPackageKzMultiSelectRequest, List<PackageKzMultiSelectDto>>
{
    private readonly IRepository<Package> _repository;
    private readonly IStringLocalizer<GetPackageKzMultiSelectRequestHandler> _localizer;

    public GetPackageKzMultiSelectRequestHandler(IRepository<Package> repository, IStringLocalizer<GetPackageKzMultiSelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);
    public async Task<List<PackageKzMultiSelectDto>> Handle(GetPackageKzMultiSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Package, PackageKzMultiSelectDto>)new PackageKzByMandantIdAndPackageTargetSpec(request.MandantId, request.packageTargetEnum), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["PackagesKz.notfound"], request.MandantId));
}
