using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Taxes;
public class GetTaxRequest : IRequest<TaxDto>
{
    public int Id { get; set; }
    public GetTaxRequest(int id) => Id = id;
}

public class GetTaxRequestHandler : IRequestHandler<GetTaxRequest, TaxDto>
{

    private readonly IRepository<Tax> _repository;
    private readonly IStringLocalizer<GetTaxRequestHandler> _localizer;
    private readonly IRepository<TaxItem> _taxItemRepository;

    public GetTaxRequestHandler(IRepository<Tax> repository, IRepository<TaxItem> taxItemRepository, IStringLocalizer<GetTaxRequestHandler> localizer) =>
        (_repository, _taxItemRepository, _localizer) = (repository, taxItemRepository, localizer);

    public async Task<TaxDto> Handle(GetTaxRequest request, CancellationToken cancellationToken)
    {
        TaxDto? taxDto = await _repository.GetBySpecAsync(
            (ISpecification<Tax, TaxDto>)new TaxByIdSpec(request.Id), cancellationToken);

        // ?? throw new NotFoundException(string.Format(_localizer["tax.notfound"], request.Id));

        if (taxDto == null) throw new NotFoundException(string.Format(_localizer["tax.notfound"], request.Id));

        var listTaxItems = await _taxItemRepository.ListAsync(
            (ISpecification<TaxItem, TaxItemDto>)new TaxItemByTaxIdSpec(request.Id), cancellationToken);

        taxDto.TaxItems = listTaxItems;

        return taxDto;
    }
}

public class TaxByIdSpec : Specification<Tax, TaxDto>, ISingleResultSpecification
{
    public TaxByIdSpec(int id) => Query.Where(x => x.Id == id);
}

public class TaxItemByTaxIdSpec : Specification<TaxItem, TaxItemDto>
{
    public TaxItemByTaxIdSpec(int id) =>
        Query.Where(x => x.TaxId == id)
             .OrderBy(x => x.Start)
             .ThenBy(x => x.End);
}


