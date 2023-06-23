using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Environment;

namespace FSH.WebApi.Application.Environment.Salutations;
public class UpdateSalutationRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; } // Für lokale auf den Mandanten angepasste Begrüßungen
    public string Name { get; set; }
    public string? LetterGreeting { get; set; }
    public int LanguageId { get; set; }
    public string? LetterClosing { get; set; }
    public int OrderNumber { get; set; }
}

public class UpdateSalutationRequestValidator : CustomValidator<UpdateSalutationRequest>
{
    public UpdateSalutationRequestValidator(IReadRepository<Salutation> repository, IStringLocalizer<UpdateSalutationRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (salutation, name, ct) =>
                await repository.GetBySpecAsync(new SalutationByNameSpec(name, salutation.MandantId), ct)
                is not Salutation existingSalutation || existingSalutation.Id == salutation.Id)
                .WithMessage((_, name) => string.Format(localizer["salutationName.alreadyexists"], name));
        RuleFor(x => x.LetterGreeting)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.LetterClosing)
            .NotEmpty()
            .MaximumLength(50);
    }
}

public class UpdateSalutationRequestHandler : IRequestHandler<UpdateSalutationRequest, int>
{
    private readonly IRepositoryWithEvents<Salutation> _repository;
    private readonly IStringLocalizer<UpdateSalutationRequestHandler> _localizer;

    public UpdateSalutationRequestHandler(IRepositoryWithEvents<Salutation> repository, IStringLocalizer<UpdateSalutationRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateSalutationRequest request, CancellationToken cancellationToken)
    {
        var salutation = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = salutation ?? throw new NotFoundException(string.Format(_localizer["salutation.notfound"], request.Id));
        salutation.Update(
            request.Name,
            request.LetterGreeting,
            request.LanguageId,
            request.LetterClosing,
            request.OrderNumber
            );

        salutation.DomainEvents.Add(EntityUpdatedEvent.WithEntity(salutation));
        await _repository.UpdateAsync(salutation, cancellationToken);

        return request.Id;
    }
}