using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Merchandises;

public class CreateMerchandiseRequestValidator : CustomValidator<CreateMerchandiseRequest>
{
    public CreateMerchandiseRequestValidator(IReadRepository<Merchandise> merchandiseRepo, IStringLocalizer<CreateMerchandiseRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (name, ct) => await merchandiseRepo.GetBySpecAsync(new MerchandiseByNameSpec(name), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["merchandise.alreadyexists"], name));

        RuleFor(p => p.MerchandiseNumber)
            .NotNull()
            .MustAsync(async (merchandise, merchandiseNumber, ct) =>
                await merchandiseRepo.GetBySpecAsync(new MerchandiseByMerchandiseNumberSpec(merchandiseNumber), ct)
                is not Merchandise existingMerchandise)
                .WithMessage((_, merchandiseNumber) => string.Format(localizer["merchandiseNumber.alreadyexists"], merchandiseNumber))
            .GreaterThan(999)
                .WithMessage((_, merchandiseNumber) => string.Format(localizer["merchandiseNumber.minimum_1000"], merchandiseNumber))
            .LessThan(100000)
                .WithMessage((_, merchandiseNumber) => string.Format(localizer["merchandiseNumber.maximum_99999"], merchandiseNumber));
    }
}
