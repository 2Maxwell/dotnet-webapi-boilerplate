using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Taxes;
public class GetTaxSelectRequest : IRequest<List<TaxSelectDto>>
{
    public int MandantId { get; set; }
    public GetTaxSelectRequest(int mandantId)
    {
        MandantId = mandantId;
    }
}

public class GetTaxSelectRequestHandler : IRequestHandler<GetTaxSelectRequest, List<TaxSelectDto>>
{
    private readonly IRepository<Tax> _repository;
    private readonly IStringLocalizer<GetTaxSelectRequestHandler> _localizer;
    private readonly IRepository<MandantSetting> _mandantSettingRepository;

    public GetTaxSelectRequestHandler(IRepository<Tax> repository, IStringLocalizer<GetTaxSelectRequestHandler> localizer, IRepository<MandantSetting> mandantSettingRepository)
    {
        _repository = repository;
        _localizer = localizer;
        _mandantSettingRepository = mandantSettingRepository;
    }

    public async Task<List<TaxSelectDto>> Handle(GetTaxSelectRequest request, CancellationToken cancellationToken)
    {
        var mandantSettingDto = await _mandantSettingRepository.GetBySpecAsync((ISpecification<MandantSetting, MandantSettingDto>)new GetMandantSettingByMandantIdSpec(request.MandantId), cancellationToken);

        List<TaxSelectDto> taxes = await _repository.ListAsync((ISpecification<Tax, TaxSelectDto>)new TaxSelectByTaxCountryIdSpec(mandantSettingDto.TaxCountryId, request.MandantId), cancellationToken);
        // ?? throw new NotFoundException(string.Format(_localizer["TaxSelect.notfound"], request.MandantId));

        return taxes;
    }
}