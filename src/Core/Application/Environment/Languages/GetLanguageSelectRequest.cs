using FSH.WebApi.Application.Environment.Salutations;
using FSH.WebApi.Domain.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.Languages;
public class GetLanguageSelectRequest : IRequest<List<LanguageSelectDto>>
{
}

public class LanguageByLanguageCodeSpec : Specification<Language, LanguageSelectDto>
{
    public LanguageByLanguageCodeSpec()
    {
        Query.OrderBy(c => c.LanguageCode);
    }
}

public class GetLanguageSelectRequestHandler : IRequestHandler<GetLanguageSelectRequest, List<LanguageSelectDto>>
{
    private readonly IRepository<Language> _repository;
    private readonly IStringLocalizer<GetLanguageSelectRequestHandler> _localizer;

    public GetLanguageSelectRequestHandler(IRepository<Language> repository, IStringLocalizer<GetLanguageSelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<LanguageSelectDto>> Handle(GetLanguageSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Language, LanguageSelectDto>)new LanguageByLanguageCodeSpec(), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["LanguageSelect.notfound"]));
}