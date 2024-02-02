using FSH.WebApi.Application.Accounting.Mandants;
using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Taxes;
public class GetTaxesRequest : IRequest<List<TaxDto>>
{
    public int MandantId { get; set; }
    public GetTaxesRequest(int mandantId)
    {
        MandantId = mandantId;
    }
}

public class GetTaxesRequestHandler : IRequestHandler<GetTaxesRequest, List<TaxDto>>
{
    private readonly IRepository<Tax> _repository;
    private readonly IStringLocalizer<GetTaxesRequestHandler> _localizer;
    private readonly IRepository<MandantSetting> _mandantSettingRepository;

    public GetTaxesRequestHandler(IRepository<Tax> repository, IStringLocalizer<GetTaxesRequestHandler> localizer, IRepository<MandantSetting> mandantSettingRepository)
    {
        _repository = repository;
        _localizer = localizer;
        _mandantSettingRepository = mandantSettingRepository;
    }

    public async Task<List<TaxDto>> Handle(GetTaxesRequest request, CancellationToken cancellationToken)
    {
        var mandantSettingDto = await _mandantSettingRepository.GetBySpecAsync((ISpecification<MandantSetting, MandantSettingDto>)new GetMandantSettingByMandantIdSpec(request.MandantId), cancellationToken);

        List<TaxDto> taxes = await _repository.ListAsync((ISpecification<Tax, TaxDto>)new TaxSelectByTaxCountryIdSpec(mandantSettingDto.TaxCountryId, request.MandantId), cancellationToken);

        return taxes;
    }
}