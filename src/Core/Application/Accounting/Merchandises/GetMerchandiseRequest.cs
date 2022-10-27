using FSH.WebApi.Application.Accounting.Merchandises;
using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Merchandises;

public class GetMerchandiseRequest : IRequest<MerchandiseDto>
{
    public int Id { get; set; }
    public GetMerchandiseRequest(int id) => Id = id;
}

public class MerchandiseByIdSpec : Specification<Merchandise, MerchandiseDto>, ISingleResultSpecification
{
    public MerchandiseByIdSpec(int id) =>
        Query.Where(p => p.Id == id);
}

public class GetMerchandiseRequestHandler : IRequestHandler<GetMerchandiseRequest, MerchandiseDto>
{
    private readonly IRepository<Merchandise> _repository;
    private readonly IStringLocalizer<GetMerchandiseRequestHandler> _localizer;

    public GetMerchandiseRequestHandler(IRepository<Merchandise> repository, IStringLocalizer<GetMerchandiseRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<MerchandiseDto> Handle(GetMerchandiseRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Merchandise, MerchandiseDto>)new MerchandiseByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["merchandise.notfound"], request.Id));
}
