using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.CashierRegisters;
public class UpdateCashierRegisterRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public decimal Stock { get; set; }
    public bool Open { get; set; }
}

public class UpdateCashierRegisterRequestValidator : CustomValidator<UpdateCashierRegisterRequest>
{
    public UpdateCashierRegisterRequestValidator(IReadRepository<CashierRegister> repository, IStringLocalizer<UpdateCashierRegisterRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(50)
        .MustAsync(async (cashierRegister, name, ct) => await repository.GetBySpecAsync(new CashierRegisterByNameSpec(name, cashierRegister.MandantId), ct)
        is not CashierRegister existingCashierRegister || existingCashierRegister.Id == cashierRegister.Id)
        .WithMessage((_, name) => string.Format(localizer["cashierRegisterName.alreadyexists"], name));

        RuleFor(x => x.Location)
        .MaximumLength(50);

        RuleFor(x => x.Stock)
        .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Open)
        .NotNull();
    }
}

public class UpdateCashierRegisterRequestHandler : IRequestHandler<UpdateCashierRegisterRequest, int>
{
    private readonly IRepository<CashierRegister> _repository;

    public UpdateCashierRegisterRequestHandler(IRepository<CashierRegister> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(UpdateCashierRegisterRequest request, CancellationToken cancellationToken)
    {
        var cashierRegister = await _repository.GetByIdAsync(request.Id, cancellationToken);
        cashierRegister.Name = request.Name;
        cashierRegister.Location = request.Location;
        cashierRegister.Stock = request.Stock;
        cashierRegister.Open = request.Open;
        await _repository.UpdateAsync(cashierRegister, cancellationToken);
        return cashierRegister.Id;
    }
}