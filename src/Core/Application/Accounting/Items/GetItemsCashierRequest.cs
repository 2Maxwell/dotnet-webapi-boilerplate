using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Application.Accounting.Taxes;
using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Items;
public class GetItemsCashierRequest : IRequest<List<ItemCashierDto>>
{
    public GetItemsCashierRequest(int mandantId, DateTime hotelDate)
    {
        MandantId = mandantId;
        HotelDate = hotelDate;
    }

    public int MandantId { get; set; }
    public DateTime HotelDate { get; set; }
}

public class GetItemsCashierRequestHandler : IRequestHandler<GetItemsCashierRequest, List<ItemCashierDto>>
{
    private readonly IRepository<Item> _repository;
    private readonly IRepository<ItemPriceTax> _repositoryItemPriceTax;
    private readonly IRepository<Tax> _repositoryTax;
    private readonly IRepository<TaxItem> _repositoryTaxItem;
    private readonly IRepository<MandantSetting> _repositoryMandantSetting;
    private readonly IStringLocalizer<GetItemsCashierRequestHandler> _localizer;

    public GetItemsCashierRequestHandler(IRepository<Item> repository, IRepository<ItemPriceTax> repositoryItemPriceTax, IRepository<Tax> repositoryTax, IRepository<TaxItem> repositoryTaxItem, IRepository<MandantSetting> repositoryMandantSetting, IStringLocalizer<GetItemsCashierRequestHandler> localizer)
    {
        _repository = repository;
        _repositoryItemPriceTax = repositoryItemPriceTax;
        _repositoryTax = repositoryTax;
        _repositoryTaxItem = repositoryTaxItem;
        _repositoryMandantSetting = repositoryMandantSetting;
        _localizer = localizer;
    }

    public async Task<List<ItemCashierDto>> Handle(GetItemsCashierRequest request, CancellationToken cancellationToken)
    {
        List<ItemCashierDto> items = await _repository.ListAsync((ISpecification<Item, ItemCashierDto>)new ItemsByMandantIdAndMandantId0WithOutAutomaticSpec(request.MandantId), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["Items.notfound"], request.MandantId));

        var mandantSettingDto = await _repositoryMandantSetting.GetBySpecAsync((ISpecification<MandantSetting, MandantSettingDto>)new GetMandantSettingByMandantIdSpec(request.MandantId), cancellationToken);

        List<TaxDto> taxesList = await _repositoryTax.ListAsync((ISpecification<Tax, TaxDto>)new TaxDtoByTaxCountryIdSpec(mandantSettingDto.TaxCountryId, request.MandantId), cancellationToken);

        foreach (var item in taxesList)
        {
            item.TaxItems = await _repositoryTaxItem.ListAsync((ISpecification<TaxItem, TaxItemDto>)new TaxItemByTaxIdSpec(item.Id), cancellationToken)
                           ?? throw new NotFoundException(string.Format(_localizer["TaxItems.notfound"], item.Id));
        }

        foreach (var item in items)
        {
            var priceTaxes = await _repositoryItemPriceTax.ListAsync(
                       (ISpecification<ItemPriceTax, ItemPriceTaxDto>)new ItemPriceTaxByItemIdSpec(item.Id), cancellationToken);

            item.Price = priceTaxes.Where(p => (p.Start <= request.HotelDate && p.End >= request.HotelDate) || (p.Start <= request.HotelDate && p.End is null)).FirstOrDefault()?.Price ?? 0;
            item.TaxId = priceTaxes.Where(p => (p.Start <= request.HotelDate && p.End >= request.HotelDate) || (p.Start <= request.HotelDate && p.End is null)).FirstOrDefault()?.TaxId ?? 0;

            // TaxRate muss auch nach Datum gesucht werden.

            item.TaxRate = taxesList.Where(t => t.Id == item.TaxId).FirstOrDefault()?.TaxItems.Where(ti => (ti.Start <= request.HotelDate && ti.End >= request.HotelDate) || (ti.Start <= request.HotelDate && ti.End is null)).FirstOrDefault()?.TaxRate ?? 0;
        }

        return items;
    }
}

public class ItemsByMandantIdAndMandantId0WithOutAutomaticSpec : Specification<Item, ItemCashierDto>
{
    public ItemsByMandantIdAndMandantId0WithOutAutomaticSpec(int mandantId) =>
        Query
        .Where(i => (i.MandantId == mandantId || i.MandantId == 0) && i.Automatic == false);
}

public class TaxByMandantIdSpec : Specification<Tax, TaxDto>
{
    public TaxByMandantIdSpec(int mandantId)
    {
        Query.Where(c => c.MandantId == mandantId);
    }
}
