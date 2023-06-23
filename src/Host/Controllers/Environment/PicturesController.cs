using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Application.Environment.Pictures;
using FSH.WebApi.Application.Hotel.Packages;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Environment;
public class PicturesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search pictures using available filters.", "")]
    public Task<List<PictureDto>> SearchAsync(SearchPicturesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new picture.", "")]
    public Task<int> CreateAsync(CreatePictureRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a picture.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdatePictureRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Products)]
    [OpenApiOperation("Delete a picture.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeletePictureRequest(id));
    }

    [HttpGet("getPictureMatchCodeMultiSelect/{mandantId:int}")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search picture matchCode for multiSelect.", "")]
    public Task<List<string>> GetPictureMatchCodeMultiSelectAsync(int mandantId)
    {
        return Mediator.Send(new GetPictureMatchCodeMultiSelectRequest(mandantId));
    }
}
