using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;
public class GetTaxSystemEnumRequest : IRequest<List<TaxSystemEnumDto>>
{
}

public class GetTaxSystemEnumRequestHandler : IRequestHandler<GetTaxSystemEnumRequest, List<TaxSystemEnumDto>>
{
    private readonly IStringLocalizer<GetTaxSystemEnumRequestHandler> _localizer;
    public GetTaxSystemEnumRequestHandler(IStringLocalizer<GetTaxSystemEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<TaxSystemEnumDto>> Handle(GetTaxSystemEnumRequest request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(TaxSystemEnum)).Cast<TaxSystemEnum>().Select(e => new TaxSystemEnumDto
        {
            Text = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class TaxSystemEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}
