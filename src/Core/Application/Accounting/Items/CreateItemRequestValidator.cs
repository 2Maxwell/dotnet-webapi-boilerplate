namespace FSH.WebApi.Application.Accounting.Items;

public class CreateItemRequestValidator : CustomValidator<CreateItemRequest>
{
    public CreateItemRequestValidator(IReadRepository<Item> repository, IStringLocalizer<CreateItemRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(30)
            .MustAsync(async (item, name, ct) =>
                await repository.GetBySpecAsync(new ItemByNameSpec(name, item.MandantId), ct) is null)
                // is not Item existingItem || existingItem.MandantId == item.MandantId || existingItem.MandantId == 0)
                .WithMessage((_, name) => localizer["ItemGroup {0} already Exists.", name]);

        RuleFor(p => p.ItemNumber)
            .NotEmpty()
            .GreaterThan(999)
            .LessThan(100000)
            .MustAsync(async (item, itemNumber, ct) =>
                await repository.GetBySpecAsync(new ItemByItemNumberSpec(itemNumber, item.MandantId), ct) is null)
                // is not Item existingItem || existingItem.MandantId == item.MandantId)
                .WithMessage((_, itemNumber) => localizer["Item {0} already Exists.", itemNumber]);

    }

}
