using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;

public class GetPackageBookingRhythmEnumRequest : IRequest<List<PackageBookingRhythmEnumDto>>
{
}

public class GetPackageBookingRhythmEnumRequestHandler : IRequestHandler<GetPackageBookingRhythmEnumRequest, List<PackageBookingRhythmEnumDto>>
{
    private readonly IStringLocalizer<GetPackageBookingRhythmEnumRequestHandler> _localizer;
    public GetPackageBookingRhythmEnumRequestHandler(IStringLocalizer<GetPackageBookingRhythmEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<PackageBookingRhythmEnumDto>> Handle(GetPackageBookingRhythmEnumRequest request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(PackageBookingRhythmEnum)).Cast<PackageBookingRhythmEnum>().Select(e => new PackageBookingRhythmEnumDto
        {
            Text = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class PackageBookingRhythmEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}
