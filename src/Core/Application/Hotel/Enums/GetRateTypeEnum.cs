using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;

public class GetRateTypeEnumRequest : IRequest<List<RateTypeEnumDto>>
{

}

public class GetRateTypeEnumRequestHandler : IRequestHandler<GetRateTypeEnumRequest, List<RateTypeEnumDto>>
{
    private readonly IStringLocalizer<GetRateTypeEnumRequestHandler> _localizer;
    public GetRateTypeEnumRequestHandler(IStringLocalizer<GetRateTypeEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<RateTypeEnumDto>> Handle(GetRateTypeEnumRequest request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(RateTypeEnum)).Cast<RateTypeEnum>().Select(e => new RateTypeEnumDto
        {
            Text = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class RateTypeEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}