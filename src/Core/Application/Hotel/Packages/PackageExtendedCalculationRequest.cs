using FSH.WebApi.Application.Accounting;
using FSH.WebApi.Application.Accounting.Items;
using FSH.WebApi.Application.Accounting.Taxes;
using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;

namespace FSH.WebApi.Application.Hotel.Packages;
public class PackageExtendedCalculationRequest : IRequest<List<BookingLineSummary>>
{
    public int MandantId { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public Pax pax { get; set; }
    public int RoomAmount { get; set; }
    public List<PriceCatDto> PriceCatDtos { get; set; }
    public List<PackageExtendDto> packageExtendDtos { get; set; }
}

public class PackageExtendedCalculationRequestHandler : IRequestHandler<PackageExtendedCalculationRequest, List<BookingLineSummary>>
{
    private readonly IReadRepository<Package> _repositoryPackage;
    private readonly IReadRepository<PackageItem> _repositoryPackageItem;
    private readonly IReadRepository<Item> _repositoryItem;
    private readonly IReadRepository<ItemPriceTax> _repositoryItemPriceTax;
    private readonly IReadRepository<Tax> _repositoryTax;

    public PackageExtendedCalculationRequestHandler(IReadRepository<Package> repositoryPackage, IReadRepository<PackageItem> repositoryPackageItem, IReadRepository<Item> repositoryItem, IReadRepository<ItemPriceTax> repositoryItemPriceTax, IReadRepository<Tax> repositoryTax)
    {
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _repositoryItem = repositoryItem;
        _repositoryItemPriceTax = repositoryItemPriceTax;
        _repositoryTax = repositoryTax;
    }

    public async Task<List<BookingLineSummary>> Handle(PackageExtendedCalculationRequest request, CancellationToken cancellationToken)
    {
        List<BookingLineSummary> bookingLineSummaries = new List<BookingLineSummary>();

        var itemsSpec = new ItemsByMandantIdAndMandantId0Spec(request.MandantId);
        List<ItemDto> ItemsList = await _repositoryItem.ListAsync(itemsSpec, cancellationToken);

        foreach (var item in ItemsList)
        {
            item.PriceTaxesDto = await _repositoryItemPriceTax.ListAsync(
                   (ISpecification<ItemPriceTax, ItemPriceTaxDto>)new ItemPriceTaxByItemIdSpec(item.Id), cancellationToken);
        }


        var taxesSpec = new TaxesByCountryIdSpec(80);
        List<TaxDto> TaxesList = await _repositoryTax.ListAsync(taxesSpec, cancellationToken);

        foreach (PackageExtendDto packageExtendDto in request.packageExtendDtos)
        {
            // jeden Tag mit dem Paket berechnen
            DateTime startDate = Convert.ToDateTime(request.Arrival);
            for (DateTime i = startDate; i < Convert.ToDateTime(request.Departure); i = i.AddDays(1))
            {
                PackageExecution packageExecution = new PackageExecution();
                packageExecution.packageExtendedDto = packageExtendDto;
                packageExecution.arrival = Convert.ToDateTime(request.Arrival);
                packageExecution.departure = Convert.ToDateTime(request.Departure);
                packageExecution.dateCurrently = i;
                packageExecution.priceCatPax = request.PriceCatDtos.Where(x => x.DatePrice.Date == i.Date).Select(x => x.RateCurrent).FirstOrDefault(); //csmDto
                packageExecution.priceBreakfast = 99;
                packageExecution.roomAmount = request.RoomAmount;
                packageExecution.adults = request.pax.Adult;
                packageExecution.children = request.pax.Children!;
                packageExecution.amountBreakfast = 1;
                packageExecution.packageAmount = packageExtendDto.Amount;
                packageExecution.bookingLineNumber = packageExtendDto.PackageDto.Kz + i.ToString("yyyyMMddHHmmssfff");
                packageExecution.itemsList = ItemsList;
                packageExecution.taxesList = TaxesList;

                if (packageExecution.isValid)
                {
                    List<BookingLine> bookingLines = packageExecution.getBookingLines;
                    BookingLineSummary blSummary = new();
                    blSummary.SourceList.AddRange(bookingLines);
                    bookingLineSummaries.Add(blSummary);
                }

            }
        }

        return bookingLineSummaries;
    }
}

