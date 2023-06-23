using FSH.WebApi.Domain.Enums;

namespace FSH.WebApi.Application.Hotel.Enums;
public class GetCompanyTypeEnumRequest : IRequest<List<CompanyTypeEnumDto>>
{
}

public class GetCompanyTypeEnumRequestHandler : IRequestHandler<GetCompanyTypeEnumRequest, List<CompanyTypeEnumDto>>
{
    private readonly IStringLocalizer<GetCompanyTypeEnumRequestHandler> _localizer;
    public GetCompanyTypeEnumRequestHandler(IStringLocalizer<GetCompanyTypeEnumRequestHandler> localizer) => _localizer = localizer;
    public Task<List<CompanyTypeEnumDto>> Handle(GetCompanyTypeEnumRequest request, CancellationToken cancellationToken)
    {
        var list = Enum.GetValues(typeof(CompanyTypeEnum)).Cast<CompanyTypeEnum>().Select(e => new CompanyTypeEnumDto
        {
            Name = e.ToString(),
            Value = (int)e
        }).ToList();

        return Task.FromResult(list);
    }
}

public class CompanyTypeEnumDto : IDto
{
    public int Value { get; set; }
    public string Name { get; set; }
}