using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;

public class GetPackageTargetStringEnum : IRequest<List<string>>
{
}

public class GetPackageTargetStringRequestHandler : IRequestHandler<GetPackageTargetStringEnum, List<string>>
{
    private readonly IStringLocalizer<GetPackageTargetStringRequestHandler> _localizer;
    public GetPackageTargetStringRequestHandler(IStringLocalizer<GetPackageTargetStringRequestHandler> localizer) => _localizer = localizer;
    public Task<List<string>> Handle(GetPackageTargetStringEnum request, CancellationToken cancellationToken)
    {
        List<string> list = ((PackageTargetEnum[])Enum.GetValues(typeof(PackageTargetEnum))).Select(c => c.ToString()).ToList();
        var item = list.FirstOrDefault(x => x == "SystemPackage");
        if (item != null) list.Remove(item);
        return Task.FromResult(list);
    }
}