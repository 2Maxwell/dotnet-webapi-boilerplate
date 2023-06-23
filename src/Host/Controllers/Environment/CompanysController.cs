using FSH.WebApi.Application.Environment.Companys;
using FSH.WebApi.Application.Environment.Persons;
using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Environment;
public class CompanysController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search companys using available filters.", "")]
    public Task<PaginationResponse<CompanySearchDto>> SearchAsync(SearchCompanysRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get company details.", "")]
    public Task<CompanyDto> GetAsync(int id)
    {
        return Mediator.Send(new GetCompanyRequest(id));
    }

    [HttpGet("getCompanyCommunication/{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get comapny communication details.", "")]
    public Task<CompanyCommunicationDto> GetCommunicationAsync(int id)
    {
        return Mediator.Send(new GetCompanyCommunicationRequest(id));
    }

    [HttpGet("getCompanySearch/{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get companySearch details.", "")]
    public Task<CompanySearchDto> GetCompanySearchAsync(int id)
    {
        return Mediator.Send(new GetCompanySearchRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new company.", "")]
    public Task<int> CreateAsync(CreateCompanyRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a company.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateCompanyRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }
}