using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Application.Hotel.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Enums;
public class EnumsController : VersionedApiController
{
    [HttpGet("getRateTypeEnum")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get RateTypeEnum as List for Select.", "")]
    public Task<List<RateTypeEnumDto>> GetRateTypeEnumRequest()
    {
        return Mediator.Send(new GetRateTypeEnumRequest());
    }

    [HttpGet("getRateScopeEnum")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get RateScopeEnum as List for Select.", "")]
    public Task<List<RateScopeEnumDto>> GetRateScopeEnumRequest()
    {
        return Mediator.Send(new GetRateScopeEnumRequest());
    }

    [HttpGet("getPackageBookingBaseEnum")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get PackageBookingBaseEnum as List for Select.", "")]
    public Task<List<PackageBookingBaseEnumDto>> GetPackageBookingBaseEnumRequest()
    {
        return Mediator.Send(new GetPackageBookingBaseEnumRequest());
    }

    [HttpGet("getPackageBookingMechanicEnum")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get PackageBookingMechanicEnum as List for Select.", "")]
    public Task<List<PackageBookingMechanicEnumDto>> GetPackageBookingMechanicEnumRequest()
    {
        return Mediator.Send(new GetPackageBookingMechanicEnumRequest());
    }

    [HttpGet("getPackageBookingRhythmEnum")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get PackageBookingRhythmEnum as List for Select.", "")]
    public Task<List<PackageBookingRhythmEnumDto>> GetPackageBookingRhythmEnumRequest()
    {
        return Mediator.Send(new GetPackageBookingRhythmEnumRequest());
    }

    [HttpGet("getPackageItemCoreValueEnum")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get PackageItemCoreValueEnum as List for Select.", "")]
    public Task<List<PackageItemCoreValueEnumDto>> GetPackageItemCoreValueEnumRequest()
    {
        return Mediator.Send(new GetPackageItemCoreValueEnumRequest());
    }

    [HttpGet("getTaxSystemEnum")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Get TaxSystemEnum as List for Select.", "")]
    public Task<List<TaxSystemEnumDto>> GetTaxSystemEnumRequest()
    {
        return Mediator.Send(new GetTaxSystemEnumRequest());
    }

}
