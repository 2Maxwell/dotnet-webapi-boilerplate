namespace FSH.WebApi.Application.Environment.Languages;

public class CreateLanguageRequest : IRequest<int>
{
    public string Name { get; set; } = default!;
    public string StartTag { get; set; }
    public string EndTag { get; set; }

    public string LanguageCode { get; set; }
}

public class CreateLanguageRequestValidator : CustomValidator<CreateLanguageRequest>
{
    public CreateLanguageRequestValidator(IReadRepository<Domain.Environment.Language> repository, IStringLocalizer<CreateLanguageRequestValidator> localizer) =>
        RuleFor(p => p.Name)
        .NotEmpty()
        .MaximumLength(20)
        .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new LanguageByNameSpec(name), ct) is null)
        .WithMessage((_, name) => string.Format(localizer["language.alreadyexists"], name));
}

public class CreateLanguageRequestHandler : IRequestHandler<CreateLanguageRequest, int>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Domain.Environment.Language> _repository;

    public CreateLanguageRequestHandler(IRepositoryWithEvents<Domain.Environment.Language> repository) => _repository = repository;

    public async Task<int> Handle(CreateLanguageRequest request, CancellationToken cancellationToken)
    {
        var language = new Domain.Environment.Language(request.Name, request.StartTag, request.EndTag, request.LanguageCode);

        await _repository.AddAsync(language, cancellationToken);

        return language.Id;
    }
}