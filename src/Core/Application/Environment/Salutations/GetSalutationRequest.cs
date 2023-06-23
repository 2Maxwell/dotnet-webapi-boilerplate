using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Salutations;
public class GetSalutationRequest : IRequest<SalutationDto>
{
    public int Id { get; set; }
    public GetSalutationRequest(int id) => Id = id;
}

public class GetSalutationRequestHandler : IRequestHandler<GetSalutationRequest, SalutationDto>
{
    private readonly IRepository<Salutation> _repository;
    private readonly IStringLocalizer<GetSalutationRequestHandler> _localizer;

    public GetSalutationRequestHandler(IRepository<Salutation> repository, IStringLocalizer<GetSalutationRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<SalutationDto> Handle(GetSalutationRequest request, CancellationToken cancellationToken)
    {
        SalutationDto? salutationDto = await _repository.GetBySpecAsync(
            (ISpecification<Salutation, SalutationDto>)new SalutationByIdSpec(request.Id), cancellationToken);

        // ?? throw new NotFoundException(string.Format(_localizer["salutation.notfound"], request.Id));

        if (salutationDto == null) throw new NotFoundException(string.Format(_localizer["salutation.notfound"], request.Id));
        return salutationDto;
    }
}

public class SalutationByIdSpec : Specification<Salutation, SalutationDto>, ISingleResultSpecification
{
    public SalutationByIdSpec(int id) => Query.Where(x => x.Id == id);
}