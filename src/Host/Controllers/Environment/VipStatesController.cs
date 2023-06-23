using FSH.WebApi.Application.Environment.Companys;
using FSH.WebApi.Application.Environment.VipStates;
using FSH.WebApi.Application.Hotel.BookingPolicys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Environment;
public class VipStatesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search vipStates using available filters.", "")]
    public Task<PaginationResponse<VipStatusDto>> SearchAsync(SearchVipStatesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get vipStatus details.", "")]
    public Task<VipStatusDto> GetAsync(int id)
    {
        return Mediator.Send(new GetVipStatusRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new vipStatus.", "")]
    public Task<int> CreateAsync(CreateVipStatusRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a vipStatus.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateVipStatusRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("getVipStatusSelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search bookingPolicys for select.", "")]
    public Task<List<VipStatusSelectDto>> GetVipStatusSelectAsync(int mandantId)
    {
        return Mediator.Send(new GetVipStatusSelectRequest(mandantId));
    }
}
