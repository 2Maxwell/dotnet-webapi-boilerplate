using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.PluGroups;

public class GetPluGroupRequest : IRequest<PluGroupDto>
{
    public int Id { get; set; }

    public GetPluGroupRequest(int id) => Id = id;
}

public class PluGroupByIdSpec : Specification<PluGroup, PluGroupDto>, ISingleResultSpecification
{
    public PluGroupByIdSpec(int id) =>
        Query.Where(p => p.Id == id);
}

public class GetPluGroupRequestHandler : IRequestHandler<GetPluGroupRequest, PluGroupDto>
{
    private readonly IRepository<PluGroup> _repository;
    private readonly IStringLocalizer<GetPluGroupRequestHandler> _localizer;

    public GetPluGroupRequestHandler(IRepository<PluGroup> repository, IStringLocalizer<GetPluGroupRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<PluGroupDto> Handle(GetPluGroupRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<PluGroup, PluGroupDto>)new PluGroupByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["pluGroup.notfound"], request.Id));
}