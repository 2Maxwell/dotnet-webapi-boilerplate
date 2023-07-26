using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Mandants;
public class GetFullMandantRequest : IRequest<MandantFullDto>
{
    public int MandantId { get; set; }
    public GetFullMandantRequest(int id) => MandantId = id;
}

public class GetMandantDetailByIdSpec : Specification<MandantDetail, MandantDetailDto>, ISingleResultSpecification
{
    public GetMandantDetailByIdSpec(int id) =>
        Query.Where(m => m.MandantId == id);
}

public class GetFullMandantRequestHandler : IRequestHandler<GetFullMandantRequest, MandantFullDto>
{
    private readonly IRepository<Mandant> _repository;
    private readonly IRepository<MandantDetail> _repositoryDetail;
    private readonly IRepository<MandantSetting> _repositorySetting;
    private readonly IRepository<MandantNumbers> _repositoryNumbers;
    private readonly IStringLocalizer<GetFullMandantRequestHandler> _localizer;

    public GetFullMandantRequestHandler(IRepository<Mandant> repository, IRepository<MandantDetail> repositoryDetail, IRepository<MandantSetting> repositorySetting, IRepository<MandantNumbers> repositoryNumbers, IStringLocalizer<GetFullMandantRequestHandler> localizer)
    {
        _repository = repository;
        _repositoryDetail = repositoryDetail;
        _repositorySetting = repositorySetting;
        _repositoryNumbers = repositoryNumbers;
        _localizer = localizer;
    }

    public async Task<MandantFullDto> Handle(GetFullMandantRequest request, CancellationToken cancellationToken)
    {
        MandantDetailDto? mandantDetailDto = await _repositoryDetail.GetBySpecAsync(
                       (ISpecification<MandantDetail, MandantDetailDto>)new GetMandantDetailByIdSpec(request.MandantId), cancellationToken);

        MandantDto? mandantDto = await _repository.GetBySpecAsync(
                       (ISpecification<Mandant, MandantDto>)new MandantByIdSpec(request.MandantId), cancellationToken);

        MandantSettingDto? mandantSettingDto = await _repositorySetting.GetBySpecAsync(
                       (ISpecification<MandantSetting, MandantSettingDto>)new GetMandantSettingByMandantIdSpec(request.MandantId), cancellationToken);

        MandantNumbersDto? mandantNumbersDto = await _repositoryNumbers.GetBySpecAsync(
                       (ISpecification<MandantNumbers, MandantNumbersDto>)new GetMandantNumbersByMandantIdSpec(request.MandantId), cancellationToken);
        MandantFullDto mandantFullDto = new MandantFullDto
        {
            MandantDto = mandantDto,
            MandantDetailDto = mandantDetailDto,
            MandantSettingDto = mandantSettingDto,
            MandantNumbersDto = mandantNumbersDto
        };

        return mandantFullDto;

    }
}

public class GetMandantNumbersByMandantIdSpec : Specification<MandantNumbers, MandantNumbersDto>, ISingleResultSpecification
{
    public GetMandantNumbersByMandantIdSpec(int id) =>
        Query.Where(m => m.MandantId == id);
}
