using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Persons;
public class GetPersonRequest : IRequest<PersonDto>
{
    public int Id { get; set; }
    public GetPersonRequest(int id) => Id = id;
}

public class GetPersonRequestHandler : IRequestHandler<GetPersonRequest, PersonDto>
{
    private readonly IRepository<Person> _repository;
    private readonly IStringLocalizer<GetPersonRequestHandler> _localizer;

    public GetPersonRequestHandler(IRepository<Person> repository, IStringLocalizer<GetPersonRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<PersonDto> Handle(GetPersonRequest request, CancellationToken cancellationToken)
    {
        PersonDto? personDto = await _repository.GetBySpecAsync(
    (ISpecification<Person, PersonDto>)new PersonByIdSpec(request.Id), cancellationToken);

        if (personDto == null) throw new NotFoundException(string.Format(_localizer["person.notfound"], request.Id));

        return personDto;
    }
}

public class PersonByIdSpec : Specification<Person, PersonDto>, ISingleResultSpecification
{
    public PersonByIdSpec(int id) => Query.Where(x => x.Id == id).Include(x => x.Salutation);
}