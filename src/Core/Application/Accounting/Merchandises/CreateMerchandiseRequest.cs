using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.Merchandises;

public class CreateMerchandiseRequest : IRequest<int>
{
    public string Name { get; set; }
    public int MerchandiseNumber { get; set; }
    public int TaxId { get; set; }
    public decimal Price { get; set; }
    public int MerchandiseGroupId { get; set; }
    public bool Automatic { get; set; } = false;
}

public class CreateMerchandiseRequestHandler : IRequestHandler<CreateMerchandiseRequest, int>
{
    private readonly IRepository<Merchandise> _repository;

    public CreateMerchandiseRequestHandler(IRepository<Merchandise> repository) =>
        _repository = repository;

    public async Task<int> Handle(CreateMerchandiseRequest request, CancellationToken cancellationToken)
    {
        var merchandise = new Merchandise(request.Name, request.MerchandiseNumber, request.TaxId, request.Price, request.MerchandiseGroupId, request.Automatic);

        merchandise.DomainEvents.Add(EntityCreatedEvent.WithEntity(merchandise));

        await _repository.AddAsync(merchandise, cancellationToken);

        return merchandise.Id;
    }

}

