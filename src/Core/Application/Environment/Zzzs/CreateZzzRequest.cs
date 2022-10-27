using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Zzzs;

public class CreateZzzRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? Display { get; set; }
}

public class CreateZzzRequestValidator : CustomValidator<CreateZzzRequest>
{
    public CreateZzzRequestValidator(IReadRepository<Zzz> repository, IStringLocalizer<CreateZzzRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (Zzz, name, ct) => await repository.GetBySpecAsync(new ZzzByNameSpec(name, Zzz.MandantId), ct) is null)
            .WithMessage((_, name) => string.Format(localizer["packageName.alreadyexists"], name));

        RuleFor(x => x.Description)
            .MaximumLength(200);
    }
}

public class ZzzByNameSpec : Specification<Zzz>, ISingleResultSpecification
{
    public ZzzByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CreateZzzRequestHandler : IRequestHandler<CreateZzzRequest, int>
{
    private readonly IRepository<Zzz> _repository;

    public CreateZzzRequestHandler(IRepository<Zzz> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateZzzRequest request, CancellationToken cancellationToken)
    {
        var zzz = new Zzz(
            request.MandantId,
            request.Name,
            request.Kz,
            request.Description,
            request.Display
            );
        zzz.DomainEvents.Add(EntityCreatedEvent.WithEntity(zzz));
        await _repository.AddAsync(zzz, cancellationToken);

        return zzz.Id;
    }
}
