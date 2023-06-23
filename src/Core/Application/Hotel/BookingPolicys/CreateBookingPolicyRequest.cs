using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.BookingPolicys;

public class CreateBookingPolicyRequest : IRequest<int>
{
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

public class CreateBookingPolicyRequestValidator : CustomValidator<CreateBookingPolicyRequest>
{
    public CreateBookingPolicyRequestValidator(IReadRepository<BookingPolicy> repository, IStringLocalizer<CreateBookingPolicyRequestValidator> localizer)
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
            await repository.GetBySpecAsync(new BookingPolicyByKzSpec(kz, bookingPolicy.MandantId), ct) is null)
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

public class BookingPolicyByNameSpec : Specification<BookingPolicy>, ISingleResultSpecification
{
    public BookingPolicyByNameSpec(string name, int mandantId) =>
        Query.Where(x => x.Name == name && (x.MandantId == mandantId || x.MandantId == 0));
}

public class BookingPolicyByKzSpec : Specification<BookingPolicy>, ISingleResultSpecification
{
    public BookingPolicyByKzSpec(string kz, int mandantId) =>
        Query.Where(x => x.Kz == kz && (x.MandantId == mandantId || x.MandantId == 0));
}

public class BookingPolicyByDefaultSpec : Specification<BookingPolicy>, ISingleResultSpecification
{
    public BookingPolicyByDefaultSpec(bool isDefault, int mandantId) =>
        Query.Where(x => x.IsDefault && (x.MandantId == mandantId || x.MandantId == 0));
}

public class CreateBookingPolicyRequestHandler : IRequestHandler<CreateBookingPolicyRequest, int>
{
    private readonly IRepository<BookingPolicy> _repository;
    public CreateBookingPolicyRequestHandler(IRepository<BookingPolicy> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateBookingPolicyRequest request, CancellationToken cancellationToken)
    {
        var bookingPolicy = new BookingPolicy(
            request.MandantId,
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
        bookingPolicy.DomainEvents.Add(EntityCreatedEvent.WithEntity(bookingPolicy));
        await _repository.AddAsync(bookingPolicy, cancellationToken);

        return bookingPolicy.Id;
    }
}