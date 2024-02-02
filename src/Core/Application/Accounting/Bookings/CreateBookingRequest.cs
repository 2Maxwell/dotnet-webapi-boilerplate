using FSH.WebApi.Application.Accounting.Mandants;
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
    public string? Source { get; set; } // Cashier #ResId, NightAudit #ResId ...
    public string? BookingLineNumberId { get; set; } // Id für zusammengehörige Zeilen
    public int TaxId { get; set; }
    public decimal TaxRate { get; set; }
    public int InvoicePos { get; set; }
    public string? ReferenceId { get; set; } // Id der Originalbuchung, SplitAmount, SplitPrice
    public int? KasseId { get; set; }
    public int State { get; set; } // 1 = booked, 9 = storno
}

public class CreateBookingRequestValidator : CustomValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.MandantId).NotEmpty();
        RuleFor(x => x.HotelDate).NotEmpty();
        RuleFor(x => x.Name).NotEmpty()
            .MaximumLength(150);
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
        RuleFor(x => x.Debit).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.ItemNumber).NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.Source).MaximumLength(100);
        RuleFor(x => x.TaxId).NotEmpty();
        RuleFor(x => x.TaxRate).NotEmpty();
        RuleFor(x => x.InvoicePos).NotEmpty();
        RuleFor(x => x.BookingLineNumberId)
            .MaximumLength(32);
        RuleFor(x => x.ReferenceId)
            .MaximumLength(100);
        RuleFor(x => x.State).NotEmpty()
            .GreaterThan(0)
            .LessThan(10);
    }
}

public class CreateBookingRequestHandler : IRequestHandler<CreateBookingRequest, int>
{
    private readonly IRepository<Booking> _repository;
    private readonly IRepository<MandantNumbers> _mandantNumbersRepository;
    private readonly IRepository<Journal> _journalRepository;

    public CreateBookingRequestHandler(IRepository<Booking> repository, IRepository<MandantNumbers> mandantNumbersRepository, IRepository<Journal> journalRepository)
    {
        _repository = repository;
        _mandantNumbersRepository = mandantNumbersRepository;
        _journalRepository = journalRepository;
    }

    public async Task<int> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        int mandantId = request.MandantId;
        GetMandantNumberRequest mNumberRequest = new(mandantId, "Journal");
        GetMandantNumberRequestHandler getMandantNumberRequestHandler = new(_mandantNumbersRepository);

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
            request.State,
            null, // InvoiceId
            request.ReferenceId,
            request.KasseId);

        // booking.DomainEvents.Add(EntityCreatedEvent.WithEntity(booking));

        // TODO: Add Journaleintrag mit JournalId in Booking eintragen

        var bookingSaved = await _repository.AddAsync(booking, cancellationToken);

        var journal = new Journal(
        booking.MandantId,
        await getMandantNumberRequestHandler.Handle(mNumberRequest, cancellationToken),
        booking.Id,
        DateTime.Now,
        booking.HotelDate,
        booking.ReservationId,
        booking.Name,
        booking.Amount,
        booking.Price,
        booking.Debit,
        booking.ItemId,
        booking.ItemNumber,
        booking.Source,
        booking.BookingLineNumberId,
        booking.TaxId,
        booking.TaxRate,
        1,
        booking.ReferenceId,
        booking.KasseId);

        await _journalRepository.AddAsync(journal, cancellationToken);

        return bookingSaved.Id;
    }
}