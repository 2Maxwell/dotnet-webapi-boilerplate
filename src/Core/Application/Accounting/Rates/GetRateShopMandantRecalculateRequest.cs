using FSH.WebApi.Application.Accounting.Items;
using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Application.Accounting.Taxes;
using FSH.WebApi.Application.Hotel;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;

namespace FSH.WebApi.Application.Accounting.Rates;
public class GetRateShopMandantRecalculateRequest : IRequest<RateShopMandantDto>
{
    public int MandantId { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public Pax pax { get; set; }
    public int RoomAmount { get; set; }
    public List<PriceCatDto> PriceCatDtos { get; set; }
    public RateShopMandantDto rateShopMandantDto { get; set; }
}

public class GetRateShopMandantRecalculateRequestHandler : IRequestHandler<GetRateShopMandantRecalculateRequest, RateShopMandantDto>
{
    private readonly IReadRepository<Package> _repositoryPackage;
    private readonly IReadRepository<PackageItem> _repositoryPackageItem;
    private readonly IReadRepository<Item> _repositoryItem;
    private readonly IReadRepository<Tax> _repositoryTax;
    private readonly IReadRepository<MandantSetting> _repositoryMandantSetting;

    public GetRateShopMandantRecalculateRequestHandler(IReadRepository<Package> repositoryPackage, IReadRepository<PackageItem> repositoryPackageItem, IReadRepository<Item> repositoryItem, IReadRepository<Tax> repositoryTax, IReadRepository<MandantSetting> repositoryMandantSetting)
    {
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _repositoryItem = repositoryItem;
        _repositoryTax = repositoryTax;
        _repositoryMandantSetting = repositoryMandantSetting;
    }

    public async Task<RateShopMandantDto> Handle(GetRateShopMandantRecalculateRequest request, CancellationToken cancellationToken)
    {
        var itemsSpec = new ItemsByMandantIdAndMandantId0Spec(request.MandantId);
        List<ItemDto> ItemsList = await _repositoryItem.ListAsync(itemsSpec, cancellationToken);

        var mandantSettingSpec = new GetMandantSettingByMandantIdSpec(request.MandantId);
        MandantSettingDto? mandantSettingDto = await _repositoryMandantSetting.GetBySpecAsync((ISpecification<MandantSetting, MandantSettingDto>)mandantSettingSpec, cancellationToken);

        var taxesSpec = new TaxesByCountryIdSpec(mandantSettingDto.TaxCountryId);
        List<TaxDto> TaxesList = await _repositoryTax.ListAsync(taxesSpec, cancellationToken);

        RateShopMandantDto rsmDto = request.rateShopMandantDto;

        rsmDto.packagesList.Clear();
        // string Packages splitten und die einzelnen Packages laden
        if (rsmDto.Packages != null || rsmDto.Packages != string.Empty)
        {
            string[] packagesKz = rsmDto.Packages.Split(',', StringSplitOptions.TrimEntries);

            for (int i = 0; i < packagesKz.Length; i++)
            {
                if (packagesKz[i] != string.Empty)
                {
                    // lade PackageDto
                    var spec = new PackageDtoByKzSpec(packagesKz[i], request.MandantId);
                    PackageDto? pack = await _repositoryPackage.GetBySpecAsync((ISpecification<Package, PackageDto>)spec, cancellationToken);

                    // lade PackageItemDto
                    List<PackageItemDto> listPackageItems = new List<PackageItemDto>();
                    pack.PackageItems = await _repositoryPackageItem.ListAsync((ISpecification<PackageItem, PackageItemDto>)new PackageItemByPackageIdSpec(pack.Id), cancellationToken);

                    // in Liste der Packages in der Rate einfügen
                    rsmDto.packagesList.Add(pack);
                }
            }
        }

        rsmDto.bookingLinesList.Clear();
        rsmDto.bookingLineSummaries.Clear();
        rsmDto.PackagesValidDisplay = string.Empty;
        // jedes Paket in Rate berechnen
        foreach (PackageDto packageDto in rsmDto.packagesList)
        {
            PackageExtendDto packageExtendDto = new PackageExtendDto();
            packageExtendDto.PackageDto = packageDto;

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
                packageExecution.packageAmount = 0;
                packageExecution.bookingLineNumber = packageExtendDto.PackageDto.Kz + i.ToString("yyyyMMddHHmmssfff");
                packageExecution.itemsList = ItemsList;
                packageExecution.taxesList = TaxesList;

                if (packageExecution.isValid)
                {
                    if (!packageDto.Display.Contains("System Package"))
                    {
                        if (!rsmDto.PackagesValidDisplay.Contains(packageDto.DisplayShort)) { rsmDto.PackagesValidDisplay += packageDto.DisplayShort.Trim() + ";"; }
                    }

                    List<BookingLine> bookingLines = packageExecution.getBookingLines;
                    rsmDto.bookingLinesList.AddRange(bookingLines);

                    BookingLineSummary blSummary = new();
                    blSummary.SourceList.AddRange(bookingLines);
                    rsmDto.bookingLineSummaries.Add(blSummary);

                }
            }
        }

        return rsmDto;
    }

}
