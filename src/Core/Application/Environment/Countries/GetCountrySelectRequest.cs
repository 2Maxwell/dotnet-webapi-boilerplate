using FSH.WebApi.Application.Environment.Salutations;
using FSH.WebApi.Domain.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.Countries;
public class GetCountrySelectRequest : IRequest<List<CountrySelectDto>>
{
}

public class CountrySpec : Specification<Country, CountrySelectDto>
{
    public CountrySpec()
    {
        Query.OrderBy(c => c.Id);
    }
}

public class GetCountrySelectRequestHandler : IRequestHandler<GetCountrySelectRequest, List<CountrySelectDto>>
{
    private readonly IRepository<Country> _repository;
    private readonly IStringLocalizer<GetCountrySelectRequestHandler> _localizer;

    public GetCountrySelectRequestHandler(IRepository<Country> repository, IStringLocalizer<GetCountrySelectRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<CountrySelectDto>> Handle(GetCountrySelectRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync((ISpecification<Country, CountrySelectDto>)new CountrySpec(), cancellationToken)
                ?? throw new NotFoundException(string.Format(_localizer["countrySelect.notfound"], "Without Filter"));
}