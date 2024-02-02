using FSH.WebApi.Application.Accounting;
using FSH.WebApi.Application.Accounting.Items;
using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Application.Accounting.Taxes;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;
using System.Text.Json;

namespace FSH.WebApi.Application.Hotel.Reservations;
public class GetReservationPackageCalculationRequest : IRequest<List<BookingLineSummary>>
{
    public int MandantId { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public ReservationDto? ReservationDto { get; set; }
}

public class GetReservationCalculationRequestHandler : IRequestHandler<GetReservationPackageCalculationRequest, List<BookingLineSummary>>
{
    private readonly IReadRepository<Package> _repositoryPackage;
    private readonly IReadRepository<PackageItem> _repositoryPackageItem;
    private readonly IReadRepository<Item> _repositoryItem;
    private readonly IRepository<ItemPriceTax> _repositoryItemPriceTax;
    private readonly IReadRepository<Tax> _repositoryTax;
    private readonly IRepository<TaxItem> _repositoryTaxItem;
    private readonly IReadRepository<MandantSetting> _repositoryMandantSetting;
    private readonly IStringLocalizer<GetReservationCalculationRequestHandler> _localizer;

    public GetReservationCalculationRequestHandler(IReadRepository<Package> repositoryPackage, IReadRepository<PackageItem> repositoryPackageItem, IReadRepository<Item> repositoryItem, IRepository<ItemPriceTax> repositoryItemPriceTax, IReadRepository<Tax> repositoryTax, IRepository<TaxItem> repositoryTaxItem, IReadRepository<MandantSetting> repositoryMandantSetting, IStringLocalizer<GetReservationCalculationRequestHandler> localizer)
    {
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _repositoryItem = repositoryItem;
        _repositoryItemPriceTax = repositoryItemPriceTax;
        _repositoryTax = repositoryTax;
        _repositoryTaxItem = repositoryTaxItem;
        _repositoryMandantSetting = repositoryMandantSetting;
        _localizer = localizer;
    }

    public async Task<List<BookingLineSummary>> Handle(GetReservationPackageCalculationRequest request, CancellationToken cancellationToken)
    {
        // var itemsSpec = new ItemsByMandantIdAndMandantId0Spec(request.MandantId);
        // List<ItemDto> itemsList = await _repositoryItem.ListAsync(itemsSpec, cancellationToken);

        var mandantSettingSpec = new GetMandantSettingByMandantIdSpec(request.MandantId);
        MandantSettingDto? mandantSettingDto = await _repositoryMandantSetting.GetBySpecAsync((ISpecification<MandantSetting, MandantSettingDto>)mandantSettingSpec, cancellationToken);

        var taxesSpec = new TaxesByCountryIdSpec(mandantSettingDto!.TaxCountryId);
        List<TaxDto>? taxesList = await _repositoryTax.ListAsync(taxesSpec, cancellationToken);

        // In taxesList für jedes TaxDto die TaxItems laden
        foreach (var item in taxesList)
        {
            item.TaxItems = new List<TaxItemDto>();
            var taxItems = await _repositoryTaxItem.ListAsync((ISpecification<TaxItem, TaxItemDto>)new TaxItemByTaxIdSpec(item.Id), cancellationToken)
                           ?? throw new NotFoundException(string.Format(_localizer["TaxItems.notfound"], item.Id));
            item.TaxItems = taxItems;
        }

        var itemsSpec = new ItemsByMandantIdAndMandantId0Spec(request.MandantId);
        List<ItemDto> itemsList = await _repositoryItem.ListAsync(itemsSpec, cancellationToken);

        foreach (var item in itemsList)
        {
            item.PriceTaxesDto = await _repositoryItemPriceTax.ListAsync((ISpecification<ItemPriceTax, ItemPriceTaxDto>)new ItemPriceTaxByItemIdSpec(item.Id), cancellationToken);       
        }

        List<PackageDto>? packageList = new List<PackageDto>();
        Pax? pax = JsonSerializer.Deserialize<Pax>(request.ReservationDto!.PaxString!);

        List<BookingLineSummary> bookingLineSummaries = new List<BookingLineSummary>();

        if (request.ReservationDto.RatePackages != null || request.ReservationDto.RatePackages != string.Empty)
        {
            string[] packagesKz = request.ReservationDto!.RatePackages!.Split(',', StringSplitOptions.TrimEntries);

            for (int i = 0; i < packagesKz.Length; i++)
            {
                if (packagesKz[i] != string.Empty)
                {
                    // lade PackageDto
                    var spec = new PackageDtoByKzSpec(packagesKz[i], request.MandantId);
                    PackageDto? pack = await _repositoryPackage.GetBySpecAsync((ISpecification<Package, PackageDto>)spec, cancellationToken);

                    // lade PackageItemDto
                    List<PackageItemDto> listPackageItems = new List<PackageItemDto>();
                    pack!.PackageItems = await _repositoryPackageItem.ListAsync((ISpecification<PackageItem, PackageItemDto>)new PackageItemByPackageIdSpec(pack.Id), cancellationToken);

                    // in Liste der Packages in der Rate einfügen
                    packageList.Add(pack);
                }
            }
        }

        request.ReservationDto.BookingLineSummaries = new List<BookingLineSummary>();

        foreach (PackageDto pack in packageList)
        {
            PackageExtendDto packageExtendDto = new PackageExtendDto();
            packageExtendDto.PackageDto = pack;

            DateOnly startDate = DateOnly.FromDateTime(request.Arrival);
            DateOnly endDate = DateOnly.FromDateTime(request.Departure);
            int counter = 0;
            for (DateOnly i = startDate; i < endDate; i = i.AddDays(1))
            {
                DateTime date = request.Arrival.AddDays(counter);
                Console.WriteLine("Package aktuelle startDate: " + i);

                PackageExecution packageExecution = new PackageExecution();
                packageExecution.packageExtendedDto = packageExtendDto;
                packageExecution.arrival = Convert.ToDateTime(request.Arrival);
                packageExecution.departure = Convert.ToDateTime(request.Departure);
                packageExecution.dateCurrently = i.ToDateTime(TimeOnly.Parse("0:00 PM"));
                //packageExecution.dateCurrently = Convert.ToDateTime(i);

                decimal priceLogis = 0;
                if (request.ReservationDto!.PriceTagDto!.RateSelected == 1) priceLogis = request.ReservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == i.ToDateTime(TimeOnly.Parse("0:00 PM")).Date).Select(x => x.RateCurrent).FirstOrDefault();
                if (request.ReservationDto!.PriceTagDto!.RateSelected == 2) priceLogis = request.ReservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == i.ToDateTime(TimeOnly.Parse("0:00 PM")).Date).Select(x => x.AverageRate).FirstOrDefault();
                if (request.ReservationDto!.PriceTagDto!.RateSelected == 3) priceLogis = request.ReservationDto!.PriceTagDto!.UserRate!.Value; // request.ReservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == i.ToDateTime(TimeOnly.Parse("0:00 PM")).Date).Select(x => x.UserRate!.Value).FirstOrDefault();
                if (priceLogis == 0 & request.ReservationDto!.PriceTagDto!.RateSelected != 3) priceLogis = request.ReservationDto!.PriceTagDto!.AverageRate;


                packageExecution.priceCatPax = priceLogis; // request.ReservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == i.Date).Select(x => x.RateCurrent).FirstOrDefault();
                packageExecution.priceBreakfast = 99;
                packageExecution.roomAmount = request.ReservationDto.RoomAmount;
                packageExecution.adults = pax!.Adult;
                packageExecution.children = pax!.Children!;
                packageExecution.amountBreakfast = 1;
                packageExecution.packageAmount = 0;
                packageExecution.bookingLineNumber = packageExtendDto.PackageDto.Kz + date.ToString("yyyyMMddHHmmssfff");
                packageExecution.itemsList = itemsList;
                packageExecution.taxesList = taxesList;

                if (packageExecution.isValid)
                {
                    List<BookingLine> bookingLines = packageExecution.getBookingLines;

                    BookingLineSummary blSummary = new();
                    blSummary.SourceList.AddRange(bookingLines);
                    bookingLineSummaries.Add(blSummary);
                }
            }
        }

        foreach (PackageExtendDto packExt in request.ReservationDto.PackageExtendOptionDtos)
        {
            DateTime startDate = Convert.ToDateTime(request.Arrival);
            for (DateTime i = startDate; i < Convert.ToDateTime(request.Departure); i = i.AddDays(1))
            {
                Console.WriteLine("PackageExtend aktuelle startDate: " + i);

                PackageExecution packageExecution = new PackageExecution();
                packageExecution.packageExtendedDto = packExt;
                packageExecution.arrival = Convert.ToDateTime(request.Arrival);
                packageExecution.departure = Convert.ToDateTime(request.Departure);
                packageExecution.dateCurrently = i;
                packageExecution.priceCatPax = request.ReservationDto!.PriceTagDto!.PriceTagDetails!.Where(x => x.DatePrice.Date == i.Date).Select(x => x.RateCurrent).FirstOrDefault();
                packageExecution.priceBreakfast = 99;
                packageExecution.roomAmount = request.ReservationDto.RoomAmount;
                packageExecution.adults = pax!.Adult;
                packageExecution.children = pax!.Children!;
                packageExecution.amountBreakfast = 1;
                packageExecution.packageAmount = packExt.Amount;
                packageExecution.bookingLineNumber = packExt.PackageDto.Kz + i.ToString("yyyyMMddHHmmssfff");
                packageExecution.itemsList = itemsList;
                packageExecution.taxesList = taxesList;

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