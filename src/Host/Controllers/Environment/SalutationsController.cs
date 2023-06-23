using FSH.WebApi.Application.Environment.Salutations;
using FSH.WebApi.Application.Hotel.BookingPolicys;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Environment;
public class SalutationsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search salutations using available filters.", "")]
    public Task<PaginationResponse<SalutationDto>> SearchAsync(SearchSalutationsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new salutation.", "")]
    public Task<int> CreateAsync(CreateSalutationRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a salutation.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateSalutationRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get salutation details.", "")]
    public Task<SalutationDto> GetAsync(int id)
    {
        return Mediator.Send(new GetSalutationRequest(id));
    }

    [HttpGet("getSalutationSelect/{mandantId:int},{languageId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search salutations for select.", "")]
    public Task<List<SalutationSelectDto>> GetSalutationSelectAsync(int mandantId, int languageId)
    {
        return Mediator.Send(new GetSalutationSelectRequest(mandantId, languageId));
    }

}
