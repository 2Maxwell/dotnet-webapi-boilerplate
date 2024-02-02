using FSH.WebApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Hotel.Enums;
public class GetDurationUnitEnum : IRequest<List<DurationUnitEnumDto>>
{
}

public class GetDurationUnitEnumHandler : IRequestHandler<GetDurationUnitEnum, List<DurationUnitEnumDto>>
{
    private readonly IStringLocalizer<GetDurationUnitEnumHandler> _localizer;
    public GetDurationUnitEnumHandler(IStringLocalizer<GetDurationUnitEnumHandler> localizer) => _localizer = localizer;
    public Task<List<DurationUnitEnumDto>> Handle(GetDurationUnitEnum request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(DurationUnitEnum)).Cast<DurationUnitEnum>().Select(e => new DurationUnitEnumDto
        {
            Text = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class DurationUnitEnumDto : IDto
{
    public int Value { get; set; }
    public string Text { get; set; }
}
