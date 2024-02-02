using FSH.WebApi.Domain.Accounting;
using Mapster;

namespace FSH.WebApi.Application.Accounting.CashierRegisters;
public class CreateCashierRegisterRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public decimal Stock { get; set; }
    public bool Open { get; set; }
}

public class CreateCashierRegisterRequestValidator : CustomValidator<CreateCashierRegisterRequest>
{
    public CreateCashierRegisterRequestValidator(IReadRepository<CashierRegister> repository, IStringLocalizer<CreateCashierRegisterRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(50)
        .MustAsync(async (cashierRegister, name, ct) => await repository.GetBySpecAsync(new CashierRegisterByNameSpec(name, cashierRegister.MandantId), ct) is null)
        .WithMessage((_, name) => string.Format(localizer["cashierRegisterName.alreadyexists"], name));

        RuleFor(x => x.Location)
        .MaximumLength(50);

        RuleFor(x => x.Stock)
        .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Open)
        .NotNull();
    }
}

public class CashierRegisterByNameSpec : Specification<CashierRegister>, ISingleResultSpecification
{
    public CashierRegisterByNameSpec(string name, int mandantId)
    {
        Query.Where(x => x.Name == name && x.MandantId == mandantId);
    }
}

public class CreateCashierRegisterRequestHandler : IRequestHandler<CreateCashierRegisterRequest, int>
{
    private readonly IRepository<CashierRegister> _repository;

    public CreateCashierRegisterRequestHandler(IRepository<CashierRegister> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateCashierRegisterRequest request, CancellationToken cancellationToken)
    {
        var cashierRegister = new CashierRegister(request.MandantId, request.Name, request.Location, request.Stock, request.Open); // request.Adapt<CashierRegister>(); // _mapper.Map<CashierRegister>(request);

        await _repository.AddAsync(cashierRegister, cancellationToken);
        return cashierRegister.Id;
    }
}
