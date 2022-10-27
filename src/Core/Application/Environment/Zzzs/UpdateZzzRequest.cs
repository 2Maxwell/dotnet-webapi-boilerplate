using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Zzzs;

public class UpdateZzzRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? Display { get; set; }
}

public class UpdateZzzRequestValidator : CustomValidator<UpdateZzzRequest>
{
    public UpdateZzzRequestValidator(IReadRepository<Zzz> repository, IStringLocalizer<UpdateZzzRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (priceSchema, name, ct) => await repository.GetBySpecAsync(new ZzzByNameSpec(name, priceSchema.MandantId), ct) is null)
            .WithMessage((_, name) => string.Format(localizer["packageName.alreadyexists"], name));

        RuleFor(x => x.Description)
            .MaximumLength(200);
    }
}

public class UpdateZzzRequestHandler : IRequestHandler<UpdateZzzRequest, int>
{
    private readonly IRepositoryWithEvents<Zzz> _repository;
    private readonly IStringLocalizer<UpdateZzzRequestHandler> _localizer;

    public UpdateZzzRequestHandler(IRepositoryWithEvents<Zzz> repository, IStringLocalizer<UpdateZzzRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateZzzRequest request, CancellationToken cancellationToken)
    {
        var zzz = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = zzz ?? throw new NotFoundException(string.Format(_localizer["zzz.notfound"], request.Id));
        zzz.Update(
            request.Name,
            request.Kz,
            request.Description,
            request.Display
            );
        zzz.DomainEvents.Add(EntityUpdatedEvent.WithEntity(zzz));
        await _repository.UpdateAsync(zzz, cancellationToken);

        return request.Id;
    }
}
