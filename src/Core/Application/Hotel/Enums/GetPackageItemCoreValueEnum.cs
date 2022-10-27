using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;

public class GetPackageItemCoreValueEnumRequest : IRequest<List<PackageItemCoreValueEnumDto>>
{
}

public class GetPackageItemCoreValueEnumRequestHandler : IRequestHandler<GetPackageItemCoreValueEnumRequest, List<PackageItemCoreValueEnumDto>>
{
    private readonly IStringLocalizer<GetPackageItemCoreValueEnumRequestHandler> _localizer;
    public GetPackageItemCoreValueEnumRequestHandler(IStringLocalizer<GetPackageItemCoreValueEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<PackageItemCoreValueEnumDto>> Handle(GetPackageItemCoreValueEnumRequest request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(PackageItemCoreValueEnum)).Cast<PackageItemCoreValueEnum>().Select(e => new PackageItemCoreValueEnumDto
        {
            Text = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class PackageItemCoreValueEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}
