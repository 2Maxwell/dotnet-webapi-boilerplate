using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;

public class GetRateScopeEnumRequest : IRequest<List<RateScopeEnumDto>>
{

}

public class GetRateScopeEnumRequestHandler : IRequestHandler<GetRateScopeEnumRequest, List<RateScopeEnumDto>>
{
    private readonly IStringLocalizer<GetRateScopeEnumRequestHandler> _localizer;
    public GetRateScopeEnumRequestHandler(IStringLocalizer<GetRateScopeEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<RateScopeEnumDto>> Handle(GetRateScopeEnumRequest request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(RateScopeEnum)).Cast<RateScopeEnum>().Select(e => new RateScopeEnumDto
        {
            Text = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class RateScopeEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}