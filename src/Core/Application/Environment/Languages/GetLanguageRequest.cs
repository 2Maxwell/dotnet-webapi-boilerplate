using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Languages;

public class GetLanguageRequest : IRequest<LanguageDto>
{
    public int Id { get; set; }
    public GetLanguageRequest(int id) => Id = id;
}

public class LanguageByIdSpec : Specification<Language, LanguageDto>, ISingleResultSpecification
{
    public LanguageByIdSpec(int id) =>
        Query.Where(p => p.Id == id);
}

public class GetLanguageRequestHandler : IRequestHandler<GetLanguageRequest, LanguageDto>
{
    private readonly IRepository<Language> _repository;
    private readonly IStringLocalizer<GetLanguageRequestHandler> _localizer;

    public GetLanguageRequestHandler(IRepository<Language> repository, IStringLocalizer<GetLanguageRequestHandler> localizer) => (_repository, _localizer) = (repository, localizer);

    public async Task<LanguageDto> Handle(GetLanguageRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Domain.Environment.Language, LanguageDto>)new LanguageByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["language.notfound"], request.Id));
}
