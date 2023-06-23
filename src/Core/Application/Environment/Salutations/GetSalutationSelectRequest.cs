using FSH.WebApi.Application.Environment.Languages;
using FSH.WebApi.Domain.Environment;
namespace FSH.WebApi.Application.Environment.Salutations;
public class GetSalutationSelectRequest : IRequest<List<SalutationSelectDto>>
{
    public int MandantId { get; set; }
    public int LanguageId { get; set; }
    public GetSalutationSelectRequest(int mandantId, int languageId)
    {
        LanguageId = languageId;
        MandantId = mandantId;
    }
}

public class SalutationByMandantIdSpec : Specification<Salutation, SalutationSelectDto>
{
    public SalutationByMandantIdSpec(int mandantId, int languageId)
    {
        if (languageId == 0)
        {
        Query.Where(c => c.MandantId == mandantId || c.MandantId == 0)
             .OrderBy(c => c.LanguageId)
             .ThenBy(c => c.OrderNumber);

        }
        else
        {
            Query.Where(c => (c.MandantId == mandantId || c.MandantId == 0) && c.LanguageId == languageId)
                 .OrderBy(c => c.LanguageId)
                 .ThenBy(c => c.OrderNumber);
        }
    }
}

public class GetSalutationSelectRequestHandler : IRequestHandler<GetSalutationSelectRequest, List<SalutationSelectDto>>
{
    private readonly IRepository<Salutation> _repository;
    private readonly IStringLocalizer<GetSalutationSelectRequestHandler> _localizer;

    public GetSalutationSelectRequestHandler(IRepository<Salutation> repository, IStringLocalizer<GetSalutationSelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<SalutationSelectDto>> Handle(GetSalutationSelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Salutation, SalutationSelectDto>)new SalutationByMandantIdSpec(request.MandantId, request.LanguageId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["salutationSelect.notfound"], request.MandantId));
}