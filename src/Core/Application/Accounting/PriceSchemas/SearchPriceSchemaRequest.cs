using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.PriceSchemas;

public class SearchPriceSchemasRequest : PaginationFilter, IRequest<PaginationResponse<PriceSchemaDto>>
{
}

public class PriceSchemasBySearchRequestSpec : EntitiesByPaginationFilterSpec<PriceSchema, PriceSchemaDto>
{
    public PriceSchemasBySearchRequestSpec(SearchPriceSchemasRequest request)
        : base(request) =>
        Query
        .Where(p => p.MandantId == request.MandantId)
        .OrderBy(p => p.Name, !request.HasOrderBy());
}

public class PriceSchemaDetailsByPriceSchemaIdSpec : Specification<PriceSchemaDetail, PriceSchemaDetailDto>
{
    public PriceSchemaDetailsByPriceSchemaIdSpec(int id) =>
        Query.Where(x => x.PriceSchemaId == id)
             .OrderBy(x => x.TargetCatPax);
}

public class SearchPriceSchemasRequestHandler : IRequestHandler<SearchPriceSchemasRequest, PaginationResponse<PriceSchemaDto>>
{
    private readonly IReadRepository<PriceSchema> _repository;
    private readonly IRepository<PriceSchemaDetail> _priceSchemaDetailRepository;

    public SearchPriceSchemasRequestHandler(IReadRepository<PriceSchema> repository, IRepository<PriceSchemaDetail> priceSchemaDetailRepository) =>
        (_repository, _priceSchemaDetailRepository) = (repository, priceSchemaDetailRepository);

    public async Task<PaginationResponse<PriceSchemaDto>> Handle(SearchPriceSchemasRequest request, CancellationToken cancellationToken)
    {
        var spec = new PriceSchemasBySearchRequestSpec(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        foreach (PriceSchemaDto ps in list)
        {
            List<PriceSchemaDetailDto> listPsd = new List<PriceSchemaDetailDto>();
            listPsd = await _priceSchemaDetailRepository.ListAsync((ISpecification<PriceSchemaDetail, PriceSchemaDetailDto>)new PriceSchemaDetailsByPriceSchemaIdSpec(ps.Id), cancellationToken);

            ps.PriceSchemaDetails = listPsd;
        }

        return new PaginationResponse<PriceSchemaDto>(list, count, request.PageNumber, request.PageSize);
    }
}