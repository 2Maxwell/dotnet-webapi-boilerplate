using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Mandants;
public class GetMandantSettingRequest : IRequest<MandantSettingDto>
{
    public GetMandantSettingRequest(int mandantId)
    {
        MandantId = mandantId;
    }

    public int MandantId { get; set; }
}

public class GetMandantSettingByMandantIdSpec : Specification<MandantSetting, MandantSettingDto>, ISingleResultSpecification
{
    public GetMandantSettingByMandantIdSpec(int mandantId) =>
        Query.Where(x => x.MandantId == mandantId);
}

public class GetMandantSettingRequestHandler : IRequestHandler<GetMandantSettingRequest, MandantSettingDto>
{
    private readonly IStringLocalizer<GetMandantSettingRequestHandler> _localizer;
    private readonly IRepository<MandantSetting> _repository;

    public GetMandantSettingRequestHandler(IRepository<MandantSetting> repository, IStringLocalizer<GetMandantSettingRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<MandantSettingDto> Handle(GetMandantSettingRequest request, CancellationToken cancellationToken)
    {
        MandantSettingDto? mandantSettingDto = await _repository.GetBySpecAsync(
            (ISpecification<MandantSetting, MandantSettingDto>)new GetMandantSettingByMandantIdSpec(request.MandantId), cancellationToken);

        if (mandantSettingDto == null) throw new NotFoundException(_localizer["MandantSetting {0} Not Found.", request.MandantId]);

        return mandantSettingDto;
    }
}