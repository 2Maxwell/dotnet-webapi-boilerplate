using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Mandants;
public class GetMandantDetailsRequest : IRequest<MandantDetailDto>
{
    public GetMandantDetailsRequest(int mandantId)
    {
        MandantId = mandantId;
    }

    public int MandantId { get; set; }
}

public class GetMandantByMAndantIdSpec : Specification<MandantDetail, MandantDetailDto>, ISingleResultSpecification
{
    public GetMandantByMAndantIdSpec(int mandantId) =>
        Query.Where(x => x.Id == mandantId);
}

public class GetMandantDetailsRequestHandler : IRequestHandler<GetMandantDetailsRequest, MandantDetailDto>
{
    private readonly IStringLocalizer<GetMandantDetailsRequestHandler> _localizer;
    private readonly IRepository<MandantDetail> _repository;

    public GetMandantDetailsRequestHandler(IRepository<MandantDetail> repository, IStringLocalizer<GetMandantDetailsRequestHandler> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<MandantDetailDto> Handle(GetMandantDetailsRequest request, CancellationToken cancellationToken)
    {
        MandantDetailDto? mandantDetailDto = await _repository.GetBySpecAsync(
                       (ISpecification<MandantDetail, MandantDetailDto>)new GetMandantByMAndantIdSpec(request.MandantId), cancellationToken);

        if (mandantDetailDto == null) throw new NotFoundException(_localizer["Mandant {0} Not Found.", request.MandantId]);

        return mandantDetailDto;
    }
}