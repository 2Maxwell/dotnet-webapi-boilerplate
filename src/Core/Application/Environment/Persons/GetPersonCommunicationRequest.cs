using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Persons;
public class GetPersonCommunicationRequest : IRequest<PersonCommunicationDto>
{
    public int Id { get; set; }
    public GetPersonCommunicationRequest(int id) => Id = id;
}

public class GetPersonCommunicationRequestHandler : IRequestHandler<GetPersonCommunicationRequest, PersonCommunicationDto>
{
    private readonly IRepository<Person> _repository;
    private readonly IStringLocalizer<GetPersonCommunicationRequestHandler> _localizer;

    public GetPersonCommunicationRequestHandler(IRepository<Person> repository, IStringLocalizer<GetPersonCommunicationRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<PersonCommunicationDto> Handle(GetPersonCommunicationRequest request, CancellationToken cancellationToken)
    {
        PersonCommunicationDto? personDto = await _repository.GetBySpecAsync(
    (ISpecification<Person, PersonCommunicationDto>)new PersonCommunicationByIdSpec(request.Id), cancellationToken);

        if (personDto == null) throw new NotFoundException(string.Format(_localizer["person.notfound"], request.Id));

        return personDto;
    }
}

public class PersonCommunicationByIdSpec : Specification<Person, PersonCommunicationDto>, ISingleResultSpecification
{
    public PersonCommunicationByIdSpec(int id) => Query.Where(x => x.Id == id);
}