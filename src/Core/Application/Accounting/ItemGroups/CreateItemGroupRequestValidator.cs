namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class CreateItemGroupRequestValidator : CustomValidator<CreateItemGroupRequest>
{
    public CreateItemGroupRequestValidator(IReadRepository<ItemGroup> repository, IStringLocalizer<CreateItemGroupRequestValidator> T)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(30)
            .MustAsync(async (itemGroup, name, ct) =>
                await repository.GetBySpecAsync(new ItemGroupByNameSpec(name, itemGroup.MandantId), ct) is null)
                .WithMessage((_, name) => T["ItemGroup {0} already Exists.", name]);

        RuleFor(p => p.OrderNumber)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(1000)
            .MustAsync(async (itemGroup, orderNumber, ct) =>
                await repository.GetBySpecAsync(new ItemGroupByOrderNumberSpec(orderNumber, itemGroup.MandantId), ct) is null)
                .WithMessage((_, name) => T["ItemGroup {0} already Exists.", name]);
    }
}
