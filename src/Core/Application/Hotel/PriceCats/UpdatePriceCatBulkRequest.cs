using FSH.WebApi.Application.Accounting.PriceSchemas;
using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.PriceCats;
public class UpdatePriceCatBulkRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PriceSchemaId { get; set; }
    public decimal BasePrice { get; set; }
    public int RateTypeEnumId { get; set; }
    public bool AddPriceAutomatic { get; set; }
    // True = Der Basispreis wird in RateStart eingetragen und RateCurrent ist RateStart + RateAutomatik
    // False = Der Basispreis wird in RateCurrent eingetragen und RateStart ist gleich RateCurrent - RateAutomatik
    public bool IsOverwriteRateType { get; set; }
}

public class UpdatePriceCatBulkRequestValidator : CustomValidator<UpdatePriceCatBulkRequest>
{
    public UpdatePriceCatBulkRequestValidator(IReadRepository<PriceCat> repository, IStringLocalizer<UpdatePriceCatBulkRequestValidator> localizer)
    {
        RuleFor(x => x.PriceSchemaId)
            .NotEmpty();
        RuleFor(x => x.BasePrice)
            .NotEmpty()
            .GreaterThan(0);
    }
}

public class UpdatePriceCatBulkRequestHandler : IRequestHandler<UpdatePriceCatBulkRequest, int>
{
    private readonly IRepositoryWithEvents<PriceCat> _repository;
    private readonly IStringLocalizer<UpdatePriceCatBulkRequestHandler> _localizer;
    private readonly IRepository<PriceSchema> _repositoryPriceSchema;
    private readonly IRepository<PriceSchemaDetail> _repositoryPriceSchemaDetail;
    private readonly IReadRepository<Category> _repositoryCategory;
    private readonly IDapperRepository _dapperRepository;

    public UpdatePriceCatBulkRequestHandler(IRepositoryWithEvents<PriceCat> repository, IStringLocalizer<UpdatePriceCatBulkRequestHandler> localizer, IRepository<PriceSchema> repositoryPriceSchema, IRepository<PriceSchemaDetail> repositoryPriceSchemaDetail, IReadRepository<Category> repositoryCategory, IDapperRepository dapperRepository)
    {
        _repository = repository;
        _localizer = localizer;
        _repositoryPriceSchema = repositoryPriceSchema;
        _repositoryPriceSchemaDetail = repositoryPriceSchemaDetail;
        _repositoryCategory = repositoryCategory;
        _dapperRepository = dapperRepository;
    }

    public async Task<int> Handle(UpdatePriceCatBulkRequest request, CancellationToken cancellationToken)
    {
        request.EndDate = Convert.ToDateTime(request.EndDate).AddDays(1);
        PriceSchemaDto? priceSchemaDto = await _repositoryPriceSchema.GetBySpecAsync(
            (ISpecification<PriceSchema, PriceSchemaDto>)new PriceSchemaByIdSpec(request.PriceSchemaId), cancellationToken);

        var listPriceSchemaDetails = await _repositoryPriceSchemaDetail.ListAsync(
            (ISpecification<PriceSchemaDetail, PriceSchemaDetailDto>)new PriceSchemaDetailsByPriceSchemaIdSpec(request.PriceSchemaId), cancellationToken);

        priceSchemaDto.PriceSchemaDetails = listPriceSchemaDetails;

        string baseCatPax = priceSchemaDto.BaseCatPax!;
        string baseCat = string.Empty;
        string basePax = string.Empty;
        if (baseCatPax != null)
        {
            basePax = baseCatPax.Substring(baseCatPax.Length - 2, 1);
            baseCat = baseCatPax.Substring(0, baseCatPax.Length - 7);
        }

        var category = await _repositoryCategory.GetBySpecAsync((ISpecification<Category, CategoryDto>)new CategoryByKzSpec(baseCat, request.MandantId), cancellationToken);

        string basePriceFormatted = request.BasePrice.ToString("0.00");
        basePriceFormatted = basePriceFormatted.Replace(',', '.');
        int changes = 0;
        string sql = string.Empty;
        string updateStr = string.Empty;
        string updateStrOverwriteRateType_Value = string.Empty;
        string updateStrOverwriteRateType_Where = string.Empty;

        if (category != null)
        {
            updateStrOverwriteRateType_Value = request.IsOverwriteRateType ? $", RateTypeEnumId = {request.RateTypeEnumId}" : string.Empty;
            updateStrOverwriteRateType_Where = request.IsOverwriteRateType ? string.Empty : $"AND RateTypeEnumId <= {request.RateTypeEnumId}";

            updateStr = request.AddPriceAutomatic ? $"RateCurrent = ({basePriceFormatted} + RateAutomatic), RateStart = {basePriceFormatted}{updateStrOverwriteRateType_Value}" : $"RateCurrent = {basePriceFormatted}, RateStart = ({basePriceFormatted} - RateAutomatic){updateStrOverwriteRateType_Value}";
            sql = $"UPDATE lnx.PriceCat SET {updateStr} WHERE MandantId = {request.MandantId} AND CategoryId = {category.Id} AND Pax = {basePax} {updateStrOverwriteRateType_Where} AND (DatePrice >= '{request.StartDate}' AND DatePrice <= '{request.EndDate}')";
            changes = await _dapperRepository.ExecuteAsync<int>(sql, cancellationToken);
        }

        foreach (PriceSchemaDetailDto psDetailDto in priceSchemaDto.PriceSchemaDetails)
        {
            string detailCatPax = psDetailDto.TargetCatPax!;
            string detailCat = string.Empty;
            string detailPax = string.Empty;
            if (detailCatPax != null)
            {
                detailPax = detailCatPax.Substring(detailCatPax.Length - 2, 1);
                detailCat = detailCatPax.Substring(0, detailCatPax.Length - 7);
            }

            var detailcategory = await _repositoryCategory.GetBySpecAsync((ISpecification<Category, CategoryDto>)new CategoryByKzSpec(detailCat, request.MandantId), cancellationToken);

            decimal detailPrice = request.BasePrice + psDetailDto.TargetDifference;

            string detailPriceFormatted = detailPrice.ToString("0.00");
            detailPriceFormatted = detailPriceFormatted.Replace(',', '.');
            string detailSql = string.Empty;

            if (detailcategory != null)
            {
                updateStr = request.AddPriceAutomatic ? $"RateCurrent = ({detailPriceFormatted} + RateAutomatic), RateStart = {detailPriceFormatted}{updateStrOverwriteRateType_Value}" : $"RateCurrent = {detailPriceFormatted}, RateStart = ({detailPriceFormatted} - RateAutomatic){updateStrOverwriteRateType_Value}";
                detailSql = $"UPDATE lnx.PriceCat SET {updateStr} WHERE MandantId = {request.MandantId} AND CategoryId = {detailcategory.Id} AND Pax = {detailPax} {updateStrOverwriteRateType_Where} AND (DatePrice >= '{request.StartDate}' AND DatePrice <= '{request.EndDate}')";
                changes += await _dapperRepository.ExecuteAsync<int>(detailSql, cancellationToken);
            }
        }

        return changes;
    }
}