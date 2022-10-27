using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;

public class GetPackageBookingBaseEnumRequest : IRequest<List<PackageBookingBaseEnumDto>>
{
}

public class GetPackageBookingBaseEnumRequestHandler : IRequestHandler<GetPackageBookingBaseEnumRequest, List<PackageBookingBaseEnumDto>>
{
    private readonly IStringLocalizer<GetPackageBookingBaseEnumRequestHandler> _localizer;
    public GetPackageBookingBaseEnumRequestHandler(IStringLocalizer<GetPackageBookingBaseEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<PackageBookingBaseEnumDto>> Handle(GetPackageBookingBaseEnumRequest request, CancellationToken cancellationToken)
    {
        //var list = Enum.GetValues(typeof(PackageBookingBaseEnum)).Cast<PackageBookingBaseEnum>().Select(e => new PackageBookingBaseEnumDto
        //{
        //    Text = e.ToString(),
        //    Value = (int)e
        //}).ToList();

        List<PackageBookingBaseEnumDto> list = ((PackageBookingBaseEnum[])Enum.GetValues(typeof(PackageBookingBaseEnum))).Select(c => new PackageBookingBaseEnumDto() { Value = (int)c, Text = c.ToString()}).ToList();

        return Task.FromResult(list);
    }
}

public class PackageBookingBaseEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}