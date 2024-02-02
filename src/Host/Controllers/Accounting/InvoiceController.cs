using FSH.WebApi.Application.Accounting.Invoices;

namespace FSH.WebApi.Host.Controllers.Accounting;
public class InvoiceController : VersionedApiController
{
    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new Invoice.", "")]
    public Task<string> CreateAsync(CreateInvoiceRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("createInvoiceSolo")]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new InvoiceSolo.", "")]
    public async Task<IActionResult> CreateInvoiceSoloAsync(CreateInvoiceSoloRequest request)
    {
        var result = Mediator.Send(request);
        return Ok(result);
    }

}
