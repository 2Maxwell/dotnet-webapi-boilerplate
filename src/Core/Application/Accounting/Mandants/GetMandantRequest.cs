using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Mandants;

public class GetMandantRequest : IRequest<MandantDto>
{
    public int Id { get; set; }
    public GetMandantRequest(int id) => Id = id;
}

public class MandantByIdSpec : Specification<Mandant, MandantDto>, ISingleResultSpecification
{
    public MandantByIdSpec(int id) =>
        Query.Where(m => m.Id == id);
}

public class GetMandantRequestHandler : IRequestHandler<GetMandantRequest, MandantDto>
{
    private readonly IRepository<Mandant> _repository;
    private readonly IStringLocalizer<GetMandantRequestHandler> _localizer;

    public GetMandantRequestHandler(IRepository<Mandant> repository, IStringLocalizer<GetMandantRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<MandantDto> Handle(GetMandantRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Mandant, MandantDto>)new MandantByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["mandant.notfound"], request.Id));
}