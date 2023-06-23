using FSH.WebApi.Application.Hotel.BookingPolicys;
using FSH.WebApi.Application.Hotel.Categorys;
using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Environment.Salutations;
public class CreateSalutationRequest : IRequest<int>
{
    public int MandantId { get; set; } // Für lokale auf den Mandanten angepasste Begrüßungen
    public string Name { get; set; }
    public string? LetterGreeting { get; set; }
    public int LanguageId { get; set; }
    public string? LetterClosing { get; set; }
    public int OrderNumber { get; set; }
}

public class CreateSalutationRequestValidator : CustomValidator<CreateSalutationRequest>
{
    public CreateSalutationRequestValidator(IReadRepository<Salutation> repository, IStringLocalizer<CreateSalutationRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
        .MustAsync(async (salutation, name, ct) => await repository.GetBySpecAsync(new SalutationByNameSpec(name, salutation.MandantId), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["salutationName.alreadyexists"], name));
        RuleFor(x => x.LetterGreeting)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.LetterClosing)
            .NotEmpty()
            .MaximumLength(50);
    }
}

public class SalutationByNameSpec : Specification<Salutation>, ISingleResultSpecification
{
    public SalutationByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CreateSalutationRequestHandler : IRequestHandler<CreateSalutationRequest, int>
{
    private readonly IRepository<Salutation> _repository;
    public CreateSalutationRequestHandler(IRepository<Salutation> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateSalutationRequest request, CancellationToken cancellationToken)
    {
        var salutation = new Salutation(
            request.MandantId,
            request.Name,
            request.LetterGreeting,
            request.LanguageId,
            request.LetterClosing,
            request.OrderNumber
            );
        salutation.DomainEvents.Add(EntityCreatedEvent.WithEntity(salutation));
        await _repository.AddAsync(salutation, cancellationToken);

        return salutation.Id;
    }
}
