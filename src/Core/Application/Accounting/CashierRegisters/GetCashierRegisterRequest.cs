using FSH.WebApi.Domain.Accounting;
using Mapster;

namespace FSH.WebApi.Application.Accounting.CashierRegisters;
public class GetCashierRegisterRequest : IRequest<CashierRegisterDto>
{
    public int Id { get; set; }
    public GetCashierRegisterRequest(int id) => Id = id;
}

public class GetCashierRegisterRequestHandler : IRequestHandler<GetCashierRegisterRequest, CashierRegisterDto>
{
    private readonly IRepository<CashierRegister> _repository;
    private readonly IStringLocalizer<GetCashierRegisterRequestHandler> _localizer;

    public GetCashierRegisterRequestHandler(IRepository<CashierRegister> repository, IStringLocalizer<GetCashierRegisterRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<CashierRegisterDto> Handle(GetCashierRegisterRequest request, CancellationToken cancellationToken)
    {
        CashierRegisterDto? cashierRegisterDto = (await _repository.GetByIdAsync(request.Id), cancellationToken).Adapt<CashierRegisterDto>();

        if (cashierRegisterDto == null) throw new NotFoundException(string.Format(_localizer["cashierRegister.notfound"], request.Id));

        return cashierRegisterDto;
    }
}
