using FluentValidation.TestHelper;
using FSH.WebApi.Application.Accounting.Rates;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Application.Hotel.CancellationPolicys;
using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Application.Hotel.Packages;
using FSH.WebApi.Application.Hotel.PriceCats;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Hotel;
using FSH.WebApi.Domain.Shop;
using Mapster;

namespace FSH.WebApi.Application.ShopMandant.ResQuerys;
public class SearchMandantResQueryRequest : ResQuery, IRequest<CategoryRatesDto>
{
    public int MandantId { get; set; }
}

public class CategoryShopMandantByMandantIdSpec : Specification<Category, CategoryShopMandantDto>
{
    public CategoryShopMandantByMandantIdSpec(int mandantId, int pax)
    {
        Query.Where(c => c.MandantId == mandantId && (c.NumberOfBeds + c.NumberOfExtraBeds) <= pax)
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

public class SearchMandantResQueryRequestHandler : IRequestHandler<SearchMandantResQueryRequest, CategoryRatesDto>
{
    private readonly IReadRepository<Category> _repositoryCategory;
    private readonly IReadRepository<PriceCat> _repositoryPriceCat;
    private readonly IReadRepository<Rate> _repositoryRate;
    private readonly IReadRepository<Package> _repositoryPackage;
    private readonly IReadRepository<PackageItem> _repositoryPackageItem;
    private readonly IReadRepository<BookingPolicy> _repositoryBookingPolicy;
    private readonly IReadRepository<CancellationPolicy> _repositoryCancellationPolicy;

    public SearchMandantResQueryRequestHandler(IReadRepository<Category> repositoryCategory, IReadRepository<Rate> repositoryRate, IReadRepository<Package> repositoryPackage, IReadRepository<PackageItem> repositoryPackageItem, IReadRepository<BookingPolicy> repositoryBookingPolicy, IReadRepository<CancellationPolicy> repositoryCancellationPolicy)
    {
        _repositoryCategory = repositoryCategory;
        _repositoryRate = repositoryRate;
        _repositoryPackage = repositoryPackage;
        _repositoryPackageItem = repositoryPackageItem;
        _repositoryBookingPolicy = repositoryBookingPolicy;
        _repositoryCancellationPolicy = repositoryCancellationPolicy;
    }

    public async Task<CategoryRatesDto> Handle(SearchMandantResQueryRequest request, CancellationToken cancellationToken)
    {
        var specCategory = new CategoryShopMandantByMandantIdSpec(request.MandantId, request.BedsTotal); // TODO Pax und Zimmeranzahl richtig auswerten
        List<CategoryShopMandantDto> listCategoryShopMandantDto = await _repositoryCategory.ListAsync(specCategory, cancellationToken);

        foreach (CategoryShopMandantDto csmDto in listCategoryShopMandantDto)
        {
            var specRate = new RateShopMandantByMandantIdSpec(request.MandantId, csmDto.Kz!);
            csmDto.RatesList = await _repositoryRate.ListAsync(specRate, cancellationToken);

            // TODO PriceCat mit Pax laden
            SearchPriceCatsShopRequest searchPriceCatsShopRequest = new();
            searchPriceCatsShopRequest.MandantId = request.MandantId;
            searchPriceCatsShopRequest.Start = request.Arrival;
            searchPriceCatsShopRequest.End = request.Departure;
            searchPriceCatsShopRequest.Pax = request.BedsTotal;
            searchPriceCatsShopRequest.CategoryId = csmDto.Id;

            SearchPriceCatsShopRequestHandler spcrHandler = new SearchPriceCatsShopRequestHandler(_repositoryPriceCat);
            csmDto.PriceCatDtos = await spcrHandler.Handle(searchPriceCatsShopRequest, cancellationToken);

            // Rate mit Daten füllen
            foreach (RateShopMandantDto rsmDto in csmDto.RatesList)
            {
                // string Packages splitten und die einzelnen Packages laden
                if (rsmDto.Packages != null || rsmDto.Packages != string.Empty)
                {
                    string[] packagesKz = rsmDto.Packages.Split(',');

                    for (int i = 0; i < packagesKz.Length; i++)
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

                rsmDto.bookingPolicyDto = await _repositoryBookingPolicy.GetBySpecAsync(
                    (ISpecification<BookingPolicy, BookingPolicyDto>)new BookingPolicyByIdSpec(rsmDto.BookingPolicyId), cancellationToken);

                rsmDto.cancellationPolicyDto = await _repositoryCancellationPolicy.GetBySpecAsync(
            (ISpecification<CancellationPolicy, CancellationPolicyDto>)new CancellationPolicyByIdSpec(rsmDto.CancellationPolicyId), cancellationToken);

            }

        }

        CategoryRatesDto categoryRatesDto = new CategoryRatesDto();
        categoryRatesDto.categoryShopMandantDtos = listCategoryShopMandantDto;

        return categoryRatesDto;
    }
}