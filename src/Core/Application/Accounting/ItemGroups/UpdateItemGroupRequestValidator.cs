namespace FSH.WebApi.Application.Accounting.ItemGroups;

public class UpdateItemGroupRequestValidator : CustomValidator<UpdateItemGroupRequest>
{
    public UpdateItemGroupRequestValidator(IReadRepository<ItemGroup> repository, IStringLocalizer<UpdateItemGroupRequestValidator> localizer)
    {
        // ItemGroupValidation
        // Name:
        // Nicht vorhanden bei gleicher MandantId oder MandantId 0
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(30)
            .MustAsync(async (itemGroup, name, ct) =>
                await repository.GetBySpecAsync(new ItemGroupByNameSpec(name, itemGroup.MandantId), ct) is null)
                // is ItemGroup existingItemGroup && existingItemGroup.MandantId == itemGroup.MandantId)
                .WithMessage((_, name) => localizer["ItemGroup {0} already Exists.", name]);

        RuleFor(p => p.OrderNumber)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(1000)
            .MustAsync(async (itemGroup, orderNumber, ct) =>
                await repository.GetBySpecAsync(new ItemGroupByOrderNumberSpec(orderNumber, itemGroup.MandantId), ct) is null)
                // is ItemGroup existingItemGroup && existingItemGroup.MandantId == itemGroup.MandantId)
                .WithMessage((_, name) => localizer["ItemGroup {0} already Exists.", name]);
    }
}
