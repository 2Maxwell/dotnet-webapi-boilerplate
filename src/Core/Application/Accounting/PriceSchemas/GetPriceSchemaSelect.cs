using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.PriceSchemas;
public class GetPriceSchemaSelectRequest : IRequest<List<PriceSchemaDto>>
{
    public int MandantId { get; set; }
    public GetPriceSchemaSelectRequest(int mandantId)
    {
        MandantId = mandantId;
    }
}

public class PriceSchemaByMandantIdSpec : Specification<PriceSchema, PriceSchemaDto>
{
    public PriceSchemaByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId)
             .OrderBy(c => c.Name);
    }
}

public class GetPriceSchemaSelectRequestHandler : IRequestHandler<GetPriceSchemaSelectRequest, List<PriceSchemaDto>>
{
    private readonly IRepository<PriceSchema> _repository;

    public GetPriceSchemaSelectRequestHandler(IRepository<PriceSchema> repository) =>
        _repository = repository;

    public async Task<List<PriceSchemaDto>> Handle(GetPriceSchemaSelectRequest request, CancellationToken cancellationToken)
    {
        List<PriceSchemaDto> list = await _repository.ListAsync((ISpecification<PriceSchema, PriceSchemaDto>)new PriceSchemaByMandantIdSpec(request.MandantId), cancellationToken);
        return list;
    }
}