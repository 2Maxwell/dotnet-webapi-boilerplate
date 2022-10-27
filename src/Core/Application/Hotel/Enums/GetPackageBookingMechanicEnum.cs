using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;

public class GetPackageBookingMechanicEnumRequest : IRequest<List<PackageBookingMechanicEnumDto>>
{
}

public class GetPackageBookingMechanicEnumRequestHandler : IRequestHandler<GetPackageBookingMechanicEnumRequest, List<PackageBookingMechanicEnumDto>>
{
    private readonly IStringLocalizer<GetPackageBookingMechanicEnumRequestHandler> _localizer;
    public GetPackageBookingMechanicEnumRequestHandler(IStringLocalizer<GetPackageBookingMechanicEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<PackageBookingMechanicEnumDto>> Handle(GetPackageBookingMechanicEnumRequest request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(PackageBookingMechanicEnum)).Cast<PackageBookingMechanicEnum>().Select(e => new PackageBookingMechanicEnumDto
        {
            Text = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class PackageBookingMechanicEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}
