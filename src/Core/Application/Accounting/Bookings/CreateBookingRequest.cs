using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Bookings;
public class CreateBookingRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public DateTime HotelDate { get; set; }
    public int ReservationId { get; set; }
    public string? Name { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public bool Debit { get; set; }
    public int ItemId { get; set; }
    public int ItemNumber { get; set; }
    public string? Source { get; set; } // PackageKz = P:Kz
    public int BookingLineNumberId { get; set; } // Id für zusammengehörige Zeilen
    public int TaxId { get; set; }
    public decimal TaxRate { get; set; }
    public int InvoicePos { get; set; }
    public int? ReferenceId { get; set; }
    public int? KasseId { get; set; }
}

public class CreateBookingRequestValidator : CustomValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.MandantId).NotEmpty();
        RuleFor(x => x.HotelDate).NotEmpty();
        RuleFor(x => x.Name).NotEmpty()
            .MaximumLength(150);
        RuleFor(x => x.Amount).NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.Price).NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Debit).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.ItemNumber).NotEmpty();
        RuleFor(x => x.Source).MaximumLength(100);
        RuleFor(x => x.TaxId).NotEmpty();
        RuleFor(x => x.TaxRate).NotEmpty();
        RuleFor(x => x.InvoicePos).NotEmpty();
    }
}

public class CreateBookingRequestHandler : IRequestHandler<CreateBookingRequest, int>
{
    private readonly IRepository<Booking> _repository;
    private readonly IRepository<MandantNumbers> _mandantNumbersRepository;
    private readonly IRepository<Journal> _journalRepository;

    public CreateBookingRequestHandler(IRepository<Booking> repository) => _repository = repository;

    public async Task<int> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {


        var booking = new Booking(
            request.MandantId,
            DateTime.Now,
            request.HotelDate,
            request.ReservationId,
            request.Name!,
            request.Amount,
            request.Price,
            request.Debit,
            request.ItemId,
            request.ItemNumber,
            request.Source!,
            request.BookingLineNumberId,
            request.TaxId,
            request.TaxRate,
            request.InvoicePos,
            1, // State
            null, // InvoiceId
            request.ReferenceId,
            request.KasseId);

        // booking.DomainEvents.Add(EntityCreatedEvent.WithEntity(booking));

        // TODO: Add Journaleintrag mit JournalId in Booking eintragen

        await _repository.AddAsync(booking, cancellationToken);
        return booking.Id;
    }
}