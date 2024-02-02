using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Helper;

namespace FSH.WebApi.Host.Controllers.Hotel;
public class PackagesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search packages using available filters.", "")]
    public Task<PaginationResponse<PackageDto>> SearchAsync(SearchPackagesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get package details.", "")]
    public Task<PackageDto> GetAsync(int id)
    {
        return Mediator.Send(new GetPackageRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new package.", "")]
    public Task<int> CreateAsync(CreatePackageRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a package.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdatePackageRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("getPackageKzMultiSelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search packages kz for multiSelect.", "")]
    public Task<List<PackageKzMultiSelectDto>> GetPackageKzMultiSelectAsync(int mandantId, PackageTargetEnum packageTargetEnum)
    {
        return Mediator.Send(new GetPackageKzMultiSelectRequest(mandantId, packageTargetEnum));
    }

    [HttpGet("getPackageExtends")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search packages kz for multiSelect.", "")]
    public Task<List<PackageExtendDto>> GetPackageExtendsAsync(int mandantId, PackageTargetEnum packageTargetEnum, DateTime startDate, DateTime endDate)
    {
        return Mediator.Send(new GetPackageExtendsRequest(mandantId, packageTargetEnum, startDate, endDate));
    }

    [HttpGet("getPackageExtendOptionsByReservation")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search packageExtendOptions by Reservation.", "")]
    public Task<List<PackageExtendDto>> GetPackageExtendOptionsByReservationAsync(int mandantId, int reservationId)
    {
        return Mediator.Send(new GetPackageExtendOptionsByReservationRequest(mandantId, reservationId));
    }

    [HttpPost("packageExtendedCalculationRequest")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("get calculated PackageExtended with List of PackageExtends", "")]
    public Task<List<BookingLineSummary>> PackageExtendedCalculationRequestAsync(PackageExtendedCalculationRequest request)
    {
        return Mediator.Send(request);
    }

}
