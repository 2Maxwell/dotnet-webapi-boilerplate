using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Bookings;
public class UpdateBookingRequest : IRequest<int>
{
    public int Id { get; set; }
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
    public int State { get; set; } // 1 = booked, 5 = Rechnung gedruckt, 9 = storno
    public int? InvoiceId { get; set; }
}

public class UpdateBookingRequestValidator : CustomValidator<UpdateBookingRequest>
{
    public UpdateBookingRequestValidator()
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.HotelDate)
            .NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);
        RuleFor(x => x.Amount)
            .NotEmpty();
        RuleFor(x => x.Price)
            .NotEmpty();
        RuleFor(x => x.Debit)
            .NotEmpty();
        RuleFor(x => x.ItemId)
            .NotEmpty();
        RuleFor(x => x.ItemNumber)
            .NotEmpty();
        RuleFor(x => x.Source)
            .MaximumLength(100);
        RuleFor(x => x.TaxId)
            .NotEmpty();
        RuleFor(x => x.TaxRate)
            .NotEmpty();
        RuleFor(x => x.InvoicePos)
            .NotEmpty();
        RuleFor(x => x.BookingLineNumberId)
            .MaximumLength(32);
        RuleFor(x => x.ReferenceId)
            .MaximumLength(100);
        RuleFor(x => x.State).NotEmpty()
            .GreaterThan(0)
            .LessThan(10);
    }
}

public class UpdateBookingRequestHandler : IRequestHandler<UpdateBookingRequest, int>
{
    private readonly IRepository<Booking> _repository;
    private readonly IRepository<MandantNumbers> _mandantNumbersRepository;
    private readonly IRepository<Journal> _journalRepository;

    public UpdateBookingRequestHandler(IRepository<Booking> repository, IRepository<MandantNumbers> mandantNumbersRepository, IRepository<Journal> journalRepository)
    {
        _repository = repository;
        _mandantNumbersRepository = mandantNumbersRepository;
        _journalRepository = journalRepository;
    }

    public async Task<int> Handle(UpdateBookingRequest request, CancellationToken cancellationToken)
    {
        var booking = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (booking == null)
        {
            throw new NotFoundException($"UpdateBookingRequest Booking not found: {request.Id}");
        }

        decimal total = booking.Amount * booking.Price;
        if ((request.Amount * request.Price) > total)
        {
            throw new Exception($"UpdateBookingRequest Amount * Price > Total: {request.Amount} * {request.Price} > {total}");
        }

        // booking.MandantId = request.MandantId;
        // booking.HotelDate = request.HotelDate;
        booking.ReservationId = request.ReservationId;
        booking.Name = request.Name!;
        booking.Amount = request.Amount;
        booking.Price = request.Price;
        booking.Debit = request.Debit;
        booking.ItemId = request.ItemId;
        booking.ItemNumber = request.ItemNumber;
        booking.Source = request.Source!;
        booking.BookingLineNumberId = request.BookingLineNumberId;
        booking.TaxId = request.TaxId;
        booking.TaxRate = request.TaxRate;
        booking.InvoicePos = request.InvoicePos;
        booking.ReferenceId = request.ReferenceId;
        booking.KasseId = request.KasseId;
        booking.State = request.State;
        booking.InvoiceId = request.InvoiceId;

        await _repository.UpdateAsync(booking, cancellationToken);

        return booking.Id;
    }
}