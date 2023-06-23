using FSH.WebApi.Application.Accounting;
using FSH.WebApi.Application.Accounting.Items;
using FSH.WebApi.Application.Accounting.Rates;
using FSH.WebApi.Application.Accounting.Taxes;
using FSH.WebApi.Application.Environment.Pictures;
using FSH.WebApi.Application.Hotel;
using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Application.Hotel.CancellationPolicys;
using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Enums;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Helper;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;

namespace FSH.WebApi.Application.ShopMandant.ResQuerys;
public class SearchMandantResQueryRequest : ResQuery, IRequest<CategoryRatesDto>
{
    public int MandantId { get; set; }
}

public class CategoryShopMandantByMandantIdSpec : Specification<Category, CategoryShopMandantDto>
{
    public CategoryShopMandantByMandantIdSpec(int mandantId, int pax)
    {
        Query.Where(c => c.MandantId == mandantId && (c.NumberOfBeds + c.NumberOfExtraBeds) >= pax)
             .OrderBy(c => (c.NumberOfBeds + c.NumberOfExtraBeds))
             .ThenBy(c => c.OrderNumber);
    }
}

public class RateShopMandantByMandantIdSpec : Specification<Rate, RateShopMandantDto>
{
    public RateShopMandantByMandantIdSpec(int mandantId, string catKz)
    {
        Query.Where(r => r.MandantId == mandantId && r.Categorys!.Contains(catKz))
             .OrderBy(r => r.Kz);
    }
}

public class RateShopMandantByMandantIdRateTypeSpec : Specification<Rate, RateShopMandantDto>
{
    public RateShopMandantByMandantIdRateTypeSpec(int mandantId, string catKz, int rateTypeEnum)
    {
        Query.Where(r => r.MandantId == mandantId && r.Categorys!.Contains(catKz) && r.RateTypeEnumId >= rateTypeEnum)
             .OrderBy(r => r.Kz);
    }
}

public class SearchMandantResQueryRequestHandler : IRequestHandler<SearchMandantResQueryRequest, CategoryRatesDto>
{
    private readonly IReadRepository<Category> _repositoryCategory;
    private readonly IReadRepository<PriceCat> _repositoryPriceCat;
    private readonly IReadRepository<Rate> _repositoryRate;
    private readonly IReadRepository<Package> _repositoryPackage;
    private readonly IReadRepository<PackageItem> _repositoryPackageItem;
    private readonly IReadRepository<BookingPolicy> _repositoryBookingPolicy;
    private readonly IReadRepository<CancellationPolicy> _repositoryCancellationPolicy;
    private readonly IReadRepository<Item> _repositoryItem;
    private readonly IReadRepository<ItemPriceTax> _repositoryItemPriceTax;
    private readonly IReadRepository<Picture> _repositoryPicture;
    private readonly IReadRepository<Tax> _repositoryTax;

    public SearchMandantResQueryRequestHandler(IReadRepository<Category> repositoryCategory, IReadRepository<PriceCat> repositoryPriceCat, IReadRepository<Rate> repositoryRate, IReadRepository<Package> repositoryPackage, IReadRepository<PackageItem> repositoryPackageItem, IReadRepository<BookingPolicy> repositoryBookingPolicy, IReadRepository<CancellationPolicy> repositoryCancellationPolicy, IReadRepository<Item> repositoryItem, IReadRepository<ItemPriceTax> repositoryItemPriceTax, IReadRepository<Picture> repositoryPicture, IReadRepository<Tax> repositoryTax)
    {
        _repositoryCategory = repositoryCategory;
        _repositoryPriceCat = repositoryPriceCat;
        _repositoryRate = repositoryRate;
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _repositoryBookingPolicy = repositoryBookingPolicy;
        _repositoryCancellationPolicy = repositoryCancellationPolicy;
        _repositoryItem = repositoryItem;
        _repositoryItemPriceTax = repositoryItemPriceTax;
        _repositoryPicture = repositoryPicture;
        _repositoryTax = repositoryTax;
    }

    public async Task<CategoryRatesDto> Handle(SearchMandantResQueryRequest request, CancellationToken cancellationToken)
    {
        // csmDto CategoryShopMandantDto
        // rsmDto RateShopMandantDto
        var specCategory = new CategoryShopMandantByMandantIdSpec(request.MandantId, request.BedsTotal); // TODO Pax und Zimmeranzahl richtig auswerten
        List<CategoryShopMandantDto> listCategoryShopMandantDto = await _repositoryCategory.ListAsync(specCategory, cancellationToken);

        var itemsSpec = new ItemsByMandantIdAndMandantId0Spec(request.MandantId);
        List<ItemDto> ItemsList = await _repositoryItem.ListAsync(itemsSpec, cancellationToken);

        foreach (var item in ItemsList)
        {
            item.PriceTaxesDto = await _repositoryItemPriceTax.ListAsync(
                   (ISpecification<ItemPriceTax, ItemPriceTaxDto>)new ItemPriceTaxByItemIdSpec(item.Id), cancellationToken);
        }

        var taxesSpec = new TaxesByCountryIdSpec(49);
        List<TaxDto> TaxesList = await _repositoryTax.ListAsync(taxesSpec, cancellationToken);

        // ZUM TESTEN
        List<BookingLineSummary> bookingLineSummaryList = new ();

        foreach (CategoryShopMandantDto csmDto in listCategoryShopMandantDto)
        {
            // TODO PriceCat mit Pax laden
            SearchPriceCatsShopRequest searchPriceCatsShopRequest = new();
            searchPriceCatsShopRequest.MandantId = request.MandantId;
            searchPriceCatsShopRequest.Start = request.Arrival;
            searchPriceCatsShopRequest.End = request.Departure;
            searchPriceCatsShopRequest.Pax = request.BedsTotal;
            searchPriceCatsShopRequest.CategoryId = csmDto.Id;

            SearchPriceCatsShopRequestHandler spcrHandler = new SearchPriceCatsShopRequestHandler(_repositoryPriceCat);
            csmDto.PriceCatDtos = await spcrHandler.Handle(searchPriceCatsShopRequest, cancellationToken);

            int minimumRateTypeEnumNeeded = csmDto.PriceCatDtos.Max(x => x.RateTypeEnumId);

            // var specRate = new RateShopMandantByMandantIdSpec(request.MandantId, csmDto.Kz!);
            // geändert damit anhand der RateTypeEnumId aus PriceCats die Raten geladen werden.
            var specRate = new RateShopMandantByMandantIdRateTypeSpec(request.MandantId, csmDto.Kz!, minimumRateTypeEnumNeeded);

            csmDto.RatesList = await _repositoryRate.ListAsync(specRate, cancellationToken);

            // ImagePath mit niedrigster Ordernummer je Kategorie laden. 
            var specImagePath = new PictureByMatchCodeOrderedSpec(csmDto.Kz!.Trim(), request.MandantId);
            var pic = await _repositoryPicture.GetBySpecAsync(specImagePath, cancellationToken);
            csmDto.ImagePath = pic != null ? pic.ImagePath : string.Empty;

            // Rate mit Daten füllen
            foreach (RateShopMandantDto rsmDto in csmDto.RatesList)
            {
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

                rsmDto.bookingPolicyDto = await _repositoryBookingPolicy.GetBySpecAsync(
                    (ISpecification<BookingPolicy, BookingPolicyDto>)new BookingPolicyByIdSpec(rsmDto.BookingPolicyId), cancellationToken);

                rsmDto.cancellationPolicyDto = await _repositoryCancellationPolicy.GetBySpecAsync(
            (ISpecification<CancellationPolicy, CancellationPolicyDto>)new CancellationPolicyByIdSpec(rsmDto.CancellationPolicyId), cancellationToken);

                rsmDto.PackagesValidDisplay = string.Empty;

                // jedes Paket in Rate berechnen
                foreach (PackageDto packageDto in rsmDto.packagesList)
                {
                    PackageExtendDto packageExtendedDto = new PackageExtendDto();
                    packageExtendedDto.PackageDto = packageDto;

                    // jeden Tag mit dem Paket berechnen
                    DateTime startDate = Convert.ToDateTime(request.Arrival);
                    for (DateTime i = startDate; i < Convert.ToDateTime(request.Departure); i = i.AddDays(1))
                    {
                        PackageExecution packageExecution = new PackageExecution();
                        packageExecution.packageExtendedDto = packageExtendedDto;
                        packageExecution.arrival = Convert.ToDateTime(request.Arrival);  //csmDto
                        packageExecution.departure = Convert.ToDateTime(request.Departure);   //csmDto
                        packageExecution.dateCurrently = i;
                        packageExecution.priceCatPax = csmDto.PriceCatDtos.Where(x => x.DatePrice.Date == i.Date).Select(x => x.RateCurrent).FirstOrDefault(); //csmDto
                        packageExecution.priceBreakfast = 99;
                        packageExecution.roomAmount = request.RoomAmount;
                        packageExecution.adults = request.RoomOccupancy[0].Adult; // TODO Adults und Childs ermittelbar machen
                        packageExecution.children = request.RoomOccupancy[0].Children;
                        packageExecution.amountBreakfast = 1;
                        packageExecution.packageAmount = 0;
                        packageExecution.bookingLineNumber = int.Parse(i.ToString("yyyyMMdd"));
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

                            // TESTEN BookingLineSummary

                            BookingLineSummary blSummary = new();
                            blSummary.SourceList.AddRange(bookingLines);
                            rsmDto.bookingLineSummaries.Add(blSummary);
                        }
                    }
                }
            }
        }

        CategoryRatesDto categoryRatesDto = new CategoryRatesDto();
        categoryRatesDto.Arrival = Convert.ToDateTime(request.Arrival);
        categoryRatesDto.Departure = Convert.ToDateTime(request.Departure);
        categoryRatesDto.Adults = request.RoomOccupancy[0].Adult;
        categoryRatesDto.Childs = request.RoomOccupancy[0].Children.Count();
        categoryRatesDto.ChildsString = string.Empty;
        foreach (Child child in request.RoomOccupancy[0].Children)
        {
            categoryRatesDto.ChildsString += child.Age + "," + child.ExtraBed.ToString() + ";";
        }

        categoryRatesDto.PromotionCode = request.PromotionCode!;
        categoryRatesDto.categoryShopMandantDtos = listCategoryShopMandantDto;
        categoryRatesDto.BedsOccupied = categoryRatesDto.Adults + categoryRatesDto.Childs;

        return categoryRatesDto;
    }
}