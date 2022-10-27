using FSH.WebApi.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Accounting.Mandants;

public class UpdateMandantRequest : IRequest<int>
{
    public int Id { get; set; }
    public string? Kz { get; set; }
    public string? Name { get; set; }
    public int GroupMember { get; set; }
    public bool GroupHead { get; set; }
}

public class UpdateMandantRequestValidator : CustomValidator<UpdateMandantRequest>
{
    public UpdateMandantRequestValidator(IReadRepository<Mandant> repository, IStringLocalizer<UpdateMandantRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

        RuleFor(x => x.Kz)
        .NotEmpty()
        .MaximumLength(10)
        .MustAsync(async (kz, ct) => await repository.GetBySpecAsync(new MandantByKzSpec(kz), ct) is null)
                .WithMessage((_, kz) => string.Format(localizer["mandantKz.alreadyexists"], kz));
    }
}

public class UpdateMandantRequestHandler : IRequestHandler<UpdateMandantRequest, int>
{
    private readonly IRepositoryWithEvents<Mandant> _repository;
    private readonly IStringLocalizer<UpdateMandantRequestHandler> _localizer;

    public UpdateMandantRequestHandler(IRepositoryWithEvents<Mandant> repository, IStringLocalizer<UpdateMandantRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateMandantRequest request, CancellationToken cancellationToken)
    {
        var mandant = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = mandant ?? throw new NotFoundException(string.Format(_localizer["mandant.notfound"], request.Id));

        mandant.Update(
            request.Name,
            request.Kz,
            request.GroupMember,
            request.GroupHead);

        await _repository.UpdateAsync(mandant, cancellationToken);
        return request.Id;
    }

}