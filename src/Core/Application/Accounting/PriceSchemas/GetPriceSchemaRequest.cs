using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.PriceSchemas;

public class GetPriceSchemaRequest : IRequest<PriceSchemaDto>
{
    public int Id { get; set; }
    public GetPriceSchemaRequest(int id) => Id = id;
}

public class GetPriceSchemaRequestHandler : IRequestHandler<GetPriceSchemaRequest, PriceSchemaDto>
{

    // private readonly HttpClient _httpClient;
    private readonly IRepository<PriceSchema> _repository;
    private readonly IStringLocalizer<GetPriceSchemaRequestHandler> _localizer;
    private readonly IRepository<PriceSchemaDetail> _priceSchemaDetailRepository;

    public GetPriceSchemaRequestHandler(IRepository<PriceSchema> repository, IRepository<PriceSchemaDetail> priceSchemaDetailRepository, IStringLocalizer<GetPriceSchemaRequestHandler> localizer) =>
        (_repository, _priceSchemaDetailRepository, _localizer) = (repository, priceSchemaDetailRepository, localizer);

    public async Task<PriceSchemaDto> Handle(GetPriceSchemaRequest request, CancellationToken cancellationToken)
    {
        PriceSchemaDto? priceSchemaDto = await _repository.GetBySpecAsync(
            (ISpecification<PriceSchema, PriceSchemaDto>)new PriceSchemaByIdSpec(request.Id), cancellationToken);

        // ?? throw new NotFoundException(string.Format(_localizer["priceSchema.notfound"], request.Id));

        if (priceSchemaDto == null) throw new NotFoundException(string.Format(_localizer["priceSchema.notfound"], request.Id));

        var listPriceSchemaDetails = await _priceSchemaDetailRepository.ListAsync(
            (ISpecification<PriceSchemaDetail, PriceSchemaDetailDto>)new PriceSchemaDetailsByPriceSchemaIdSpec(request.Id), cancellationToken);

        priceSchemaDto.PriceSchemaDetails = listPriceSchemaDetails;

        return priceSchemaDto;
    }
}

public class PriceSchemaByIdSpec : Specification<PriceSchema, PriceSchemaDto>, ISingleResultSpecification
{
    public PriceSchemaByIdSpec(int id) => Query.Where(x => x.Id == id);
}

