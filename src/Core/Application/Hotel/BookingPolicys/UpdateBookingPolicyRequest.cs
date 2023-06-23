using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.BookingPolicys;

public class UpdateBookingPolicyRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
    public string? Name { get; set; }
    public string? Kz { get; set; }
    public string? Description { get; set; }
    public string? Display { get; set; }
    public string? DisplayShort { get; set; }
    public string? DisplayHighLight { get; set; }
    public string? ConfirmationText { get; set; }
    public bool IsDefault { get; set; }
    public bool CreditCard { get; set; }
    public bool Deposit { get; set; }
    public int Priority { get; set; }
    public string? ChipIcon { get; set; }
    public string? ChipText { get; set; }
}

public class UpdateBookingPolicyRequestValidator : CustomValidator<UpdateBookingPolicyRequest>
{
    public UpdateBookingPolicyRequestValidator(IReadRepository<BookingPolicy> repository, IStringLocalizer<UpdateBookingPolicyRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (bookingPolicy, name, ct) =>
            await repository.GetBySpecAsync(new BookingPolicyByNameSpec(name, bookingPolicy.MandantId), ct)
            is not BookingPolicy existingBookingPolicy || existingBookingPolicy.MandantId == bookingPolicy.MandantId)
            .WithMessage((_, name) => string.Format(localizer["bookingPolicyName.alreadyexists"], name));

        RuleFor(x => x.Kz)
            .NotEmpty()
            .MaximumLength(10)
            .MustAsync(async (bookingPolicy, kz, ct) =>
            await repository.GetBySpecAsync(new BookingPolicyByKzSpec(kz, bookingPolicy.MandantId), ct)
            is not BookingPolicy existingBookingPolicy || existingBookingPolicy.MandantId == bookingPolicy.MandantId)
            .WithMessage((_, kz) => string.Format(localizer["bookingPolicyKz.alreadyexists"], kz));

        RuleFor(x => x.Description)
            .MaximumLength(200);
        RuleFor(x => x.Display)
            .MaximumLength(500);
        RuleFor(x => x.DisplayShort)
            .MaximumLength(300);
        RuleFor(x => x.DisplayHighLight)
            .MaximumLength(300);
        RuleFor(x => x.ConfirmationText)
            .MaximumLength(500);
        RuleFor(x => x.ChipIcon)
            .MaximumLength(100);
        RuleFor(x => x.ChipText)
            .MaximumLength(50);
    }
}

public class UpdateBookingPolicyRequestHandler : IRequestHandler<UpdateBookingPolicyRequest, int>
{
    private readonly IRepositoryWithEvents<BookingPolicy> _repository;
    private readonly IStringLocalizer<UpdateBookingPolicyRequestHandler> _localizer;

    public UpdateBookingPolicyRequestHandler(IRepositoryWithEvents<BookingPolicy> repository, IStringLocalizer<UpdateBookingPolicyRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(UpdateBookingPolicyRequest request, CancellationToken cancellationToken)
    {
        var bookingPolicy = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = bookingPolicy ?? throw new NotFoundException(string.Format(_localizer["bookingPolicy.notfound"], request.Id));
        bookingPolicy.Update(
            request.Name,
            request.Kz,
            request.Description,
            request.Display,
            request.DisplayShort,
            request.DisplayHighLight,
            request.ConfirmationText,
            request.IsDefault,
            request.CreditCard,
            request.Deposit,
            request.Priority,
            request.ChipIcon,
            request.ChipText
            );

        bookingPolicy.DomainEvents.Add(EntityUpdatedEvent.WithEntity(bookingPolicy));
        await _repository.UpdateAsync(bookingPolicy, cancellationToken);

        return request.Id;
    }
}