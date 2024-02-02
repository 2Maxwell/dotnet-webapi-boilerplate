using FSH.WebApi.Domain.Accounting;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.CashierRegisters;
public class GetCashierRegisterDtosRequest : IRequest<List<CashierRegisterDto>>
{
    public GetCashierRegisterDtosRequest(int mandantId)
    {
        MandantId = mandantId;
    }

    public int MandantId { get; set; }
}

public class GetCashierRegisterDtosRequestHandler : IRequestHandler<GetCashierRegisterDtosRequest, List<CashierRegisterDto>>
{
    private readonly IRepository<CashierRegister> _repository;
    private readonly IStringLocalizer<GetCashierRegisterDtosRequestHandler> _localizer;

    public GetCashierRegisterDtosRequestHandler(IRepository<CashierRegister> repository, IStringLocalizer<GetCashierRegisterDtosRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<List<CashierRegisterDto>> Handle(GetCashierRegisterDtosRequest request, CancellationToken cancellationToken)
    {
        List<CashierRegisterDto>? cashierRegisterDtos = await _repository.ListAsync((ISpecification<CashierRegister, CashierRegisterDto>)
            new CashierRegisterByMandantIdRequestSpec(request.MandantId), cancellationToken);

        if (cashierRegisterDtos == null) throw new NotFoundException(string.Format(_localizer["cashierRegister.notfound"], request.MandantId));

        return cashierRegisterDtos;
    }
}

public class CashierRegisterByMandantIdRequestSpec : Specification<CashierRegister, CashierRegisterDto>
{
    public CashierRegisterByMandantIdRequestSpec(int MandantId)
    {
        Query
        .Where(c => c.MandantId == MandantId)
        .OrderBy(c => c.Id);
    }
}