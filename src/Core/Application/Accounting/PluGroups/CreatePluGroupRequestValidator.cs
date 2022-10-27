using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FSH.WebApi.Application.Accounting.PluGroups;

public class CreatePluGroupRequestValidator : CustomValidator<CreatePluGroupRequest>
{

    public CreatePluGroupRequestValidator(IReadRepository<PluGroup> pluGroupRepo, IStringLocalizer<CreatePluGroupRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(30)
            .MustAsync(async (name, ct) => await pluGroupRepo.GetBySpecAsync(new PluGroupByNameSpec(name), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["pluGroup.alreadyexists"], name));

        RuleFor(p => p.OrderNumber)
            .NotNull()
            .MustAsync(async (pluGroup, orderNumber, ct) =>
                await pluGroupRepo.GetBySpecAsync(new PluGroupByOrderNumberSpec(orderNumber), ct)
                is not PluGroup existingPluGroup)
            .WithMessage((_, orderNumber) => string.Format(localizer["pluGroupOrderNumber.alreadyexists"], orderNumber));

        // RuleFor(p => p.Rate)
        //    .GreaterThanOrEqualTo(1);

        // RuleFor(p => p.Image)
        //    .SetNonNullableValidator(new FileUploadRequestValidator());

        // RuleFor(p => p.BrandId)
        //    .NotEmpty()
        //    .MustAsync(async (id, ct) => await brandRepo.GetByIdAsync(id, ct) is not null)
        //        .WithMessage((_, id) => string.Format(localizer["brand.notfound"], id));
    }

}
