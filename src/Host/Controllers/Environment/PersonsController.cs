using FSH.WebApi.Application.Environment.Persons;

namespace FSH.WebApi.Host.Controllers.Environment;
public class PersonsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search persons using available filters.", "")]
    public Task<PaginationResponse<PersonDto>> SearchAsync(SearchPersonsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("searchContact")]
    [MustHavePermission(FSHAction.Search, FSHResource.Brands)]
    [OpenApiOperation("Search contacts using available filters.", "")]
    public Task<PaginationResponse<ContactDto>> SearchContactsAsync(SearchContactsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get person details.", "")]
    public Task<PersonDto> GetAsync(int id)
    {
        return Mediator.Send(new GetPersonRequest(id));
    }

    [HttpGet("getPersonCommunication/{id:int}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get person communication details.", "")]
    public Task<PersonCommunicationDto> GetCommunicationAsync(int id)
    {
        return Mediator.Send(new GetPersonCommunicationRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new person.", "")]
    public Task<int> CreateAsync(CreatePersonRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a person.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdatePersonRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }
}