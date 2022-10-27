using FSH.WebApi.Application.Catalog.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Environment.Languages;

public class UpdateLanguageRequest : IRequest<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string StartTag { get; set; }
    public string EndTag { get; set; }
}

public class UpdateLanguageRequestValidator : CustomValidator<UpdateLanguageRequest>
{
    public UpdateLanguageRequestValidator(IRepository<Domain.Environment.Language> repository, IStringLocalizer<UpdateLanguageRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (language, name, ct) =>
                    await repository.GetBySpecAsync(new LanguageByNameSpec(name), ct)
                        is not Domain.Environment.Language existingLanguage || existingLanguage.Id == language.Id)
                .WithMessage((_, name) => string.Format(localizer["language.alreadyexists"], name));
}

public class UpdateLanguageRequestHandler : IRequestHandler<UpdateLanguageRequest, int>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Domain.Environment.Language> _repository;
    private readonly IStringLocalizer<UpdateLanguageRequestHandler> _localizer;

    public UpdateLanguageRequestHandler(IRepositoryWithEvents<Domain.Environment.Language> repository, IStringLocalizer<UpdateLanguageRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateLanguageRequest request, CancellationToken cancellationToken)
    {
        var language = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = language ?? throw new NotFoundException(string.Format(_localizer["language.notfound"], request.Id));

        language.Update(request.Name, request.StartTag, request.EndTag);

        await _repository.UpdateAsync(language, cancellationToken);

        return request.Id;
    }
}