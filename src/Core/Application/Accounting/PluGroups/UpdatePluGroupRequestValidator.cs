using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.PluGroups;

public class UpdatePluGroupRequestValidator : CustomValidator<UpdatePluGroupRequest>
{
    public UpdatePluGroupRequestValidator(IReadRepository<PluGroup> pluGroupRepo, IStringLocalizer<UpdatePluGroupRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(30)
            .MustAsync(async (pluGroup, name, ct) =>
                await pluGroupRepo.GetBySpecAsync(new PluGroupByNameSpec(name), ct)
                is not PluGroup existingPluGroup || existingPluGroup.Id == pluGroup.Id)
        .WithMessage((_, name) => string.Format(localizer["pluGroup.alreadyexists"], name));

        RuleFor(p => p.OrderNumber)
            .NotNull()
            .MustAsync(async (pluGroup, orderNumber, ct) =>
                await pluGroupRepo.GetBySpecAsync(new PluGroupByOrderNumberSpec(orderNumber), ct)
                is not PluGroup existingPluGroup)
            .WithMessage((_, orderNumber) => string.Format(localizer["pluGroupOrderNumber.alreadyexists"], orderNumber));

    }
}
