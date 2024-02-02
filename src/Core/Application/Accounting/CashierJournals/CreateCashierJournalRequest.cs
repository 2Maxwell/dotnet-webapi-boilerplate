using FSH.WebApi.Domain.Accounting;
using Mapster;

namespace FSH.WebApi.Application.Accounting.CashierJournals;
public class CreateCashierJournalRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public int JournalId { get; set; }
    public int JournalIdMandant { get; set; }
    public int BookingId { get; set; }
    public int? InvoiceId { get; set; }
    public int? InvoiceIdMandant { get; set; }
    public DateTime JournalDate { get; set; }
    public DateTime HotelDate { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public bool Debit { get; set; }
    public int ItemId { get; set; }
    public int ItemNumber { get; set; }
    public string? Source { get; set; }
    public int State { get; set; }
    public DateTime StateDate { get; set; }
    public int KasseId { get; set; }
}

public class CreateCashierJournalRequestValidator : CustomValidator<CreateCashierJournalRequest>
{
    public CreateCashierJournalRequestValidator()
    {
        RuleFor(x => x.MandantId).NotEmpty();
        //RuleFor(x => x.JournalId).NotEmpty();
        //RuleFor(x => x.JournalIdMandant).NotEmpty();
        //RuleFor(x => x.BookingId).NotEmpty();
        //RuleFor(x => x.JournalDate).NotEmpty();
        //RuleFor(x => x.HotelDate).NotEmpty();
        //RuleFor(x => x.Name).NotEmpty();
        //RuleFor(x => x.Amount).NotEmpty();
        //RuleFor(x => x.Price).NotEmpty();
        //RuleFor(x => x.Debit).NotEmpty();
        //RuleFor(x => x.ItemId).NotEmpty();
        //RuleFor(x => x.ItemNumber).NotEmpty();
        //RuleFor(x => x.Source).NotEmpty();
        //RuleFor(x => x.State).NotEmpty();
        //RuleFor(x => x.StateDate).NotEmpty();
        //RuleFor(x => x.KasseId).NotEmpty();
    }
}

public class CreateCashierJournalRequestHandler : IRequestHandler<CreateCashierJournalRequest, int>
{
    private readonly IRepository<CashierJournal> _repository; // IUnitOfWork _unitOfWork;
    public CreateCashierJournalRequestHandler(IRepository<CashierJournal> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateCashierJournalRequest request, CancellationToken cancellationToken)
    {
        var cashierJournal = new CashierJournal(
            request.MandantId,
            request.JournalId,
            request.JournalIdMandant,
            request.BookingId,
            request.InvoiceId,
            request.InvoiceIdMandant,
            request.JournalDate,
            request.HotelDate,
            request.Name,
            request.Amount,
            request.Price,
            request.Debit,
            request.ItemId,
            request.ItemNumber,
            request.Source,
            request.State,
            request.StateDate,
            request.KasseId);

        await _repository.AddAsync(cashierJournal, cancellationToken); //_unitOfWork.CashierJournals.AddAsync(cashierJournal);
        return cashierJournal.Id;
    }
}