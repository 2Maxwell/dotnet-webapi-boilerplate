using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;
public class GetPackageTargetEnumRequest : IRequest<List<PackageTargetEnumDto>>
{
}

public class GetPackageTargetEnumRequestHandler : IRequestHandler<GetPackageTargetEnumRequest, List<PackageTargetEnumDto>>
{
    private readonly IStringLocalizer<GetPackageTargetEnumRequestHandler> _localizer;
    public GetPackageTargetEnumRequestHandler(IStringLocalizer<GetPackageTargetEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<PackageTargetEnumDto>> Handle(GetPackageTargetEnumRequest request, CancellationToken cancellationToken)
    {
        List<PackageTargetEnumDto> list = ((PackageTargetEnum[])Enum.GetValues(typeof(PackageTargetEnum))).Select(c => new PackageTargetEnumDto() { Value = (int)c, Text = c.ToString() }).ToList();
        var item = list.FirstOrDefault(x => x.Text == "SystemPackage");
        if(item != null) list.Remove(item);
        return Task.FromResult(list);
    }
}

public class PackageTargetEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}

