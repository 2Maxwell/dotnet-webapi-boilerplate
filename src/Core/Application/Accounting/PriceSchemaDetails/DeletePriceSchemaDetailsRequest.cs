using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.PriceSchemaDetails;

public class DeletePriceSchemaDetailRequest : IRequest<int>
{
    public int Id { get; set; }
    public DeletePriceSchemaDetailRequest(int id) => Id = id;
}

public class DeletePriceSchemaDetailsRequestHandler : IRequestHandler<DeletePriceSchemaDetailRequest, int>
{
    private readonly IRepository<PriceSchemaDetail> _repository;
    private readonly IStringLocalizer _t;

    public DeletePriceSchemaDetailsRequestHandler(IRepository<PriceSchemaDetail> repository, IStringLocalizer<DeletePriceSchemaDetailsRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<int> Handle(DeletePriceSchemaDetailRequest request, CancellationToken cancellationToken)
    {
        var priceSchemaDetail = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = priceSchemaDetail ?? throw new NotFoundException(_t["PriceSchemaDetail {0} Not Found."]);
        priceSchemaDetail.DomainEvents.Add(EntityDeletedEvent.WithEntity(priceSchemaDetail));
        await _repository.DeleteAsync(priceSchemaDetail, cancellationToken);
        return request.Id;
    }
}