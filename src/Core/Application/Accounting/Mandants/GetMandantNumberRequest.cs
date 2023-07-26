using FSH.WebApi.Domain.Accounting;

namespace FSH.WebApi.Application.Accounting.Mandants;
public class GetMandantNumberRequest : IRequest<int>
{
    public GetMandantNumberRequest(int mandantId, string numberTyp)
    {
        MandantId = mandantId;
        NumberTyp = numberTyp;
    }

    public int MandantId { get; set; }
    public string NumberTyp { get; set; } // GroupMasterNumber, PhantomNumber, InvoiceNumber, JournalNumber, ReservationNumber,
}

public class MandantNumbersByMandantIdSpec : Specification<MandantNumbers>, ISingleResultSpecification<MandantNumbers>
{
    public MandantNumbersByMandantIdSpec(int mandantId) =>
        Query.Where(x => x.MandantId == mandantId);
}

public class GetMandantNumberRequestHandler : IRequestHandler<GetMandantNumberRequest, int>
{
    private readonly IRepository<MandantNumbers> _repository;

    public GetMandantNumberRequestHandler(IRepository<MandantNumbers> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(GetMandantNumberRequest request, CancellationToken cancellationToken)
    {
        var numbers = await _repository.ListAsync(new MandantNumbersByMandantIdSpec(request.MandantId), cancellationToken);
        MandantNumbers resultNumbers;
        int result = 0;
        switch (request.NumberTyp)
        {
            case "GroupMaster":
                resultNumbers = numbers[0]!.UpdateGroupMasterNumber();
                result = resultNumbers.GroupMasterNumber;
                break;
            case "Phantom":
                resultNumbers = numbers[0]!.UpdatePhantomNumber();
                result = resultNumbers.PhantomNumber;
                break;
            case "Invoice":
                resultNumbers = numbers[0]!.UpdateInvoiceNumber();
                result = resultNumbers.InvoiceNumber;
                break;
            case "Journal":
                resultNumbers = numbers[0]!.UpdateJournalNumber();
                result = resultNumbers.JournalNumber;
                break;
            case "Reservation":
                resultNumbers = numbers[0]!.UpdateReservationNumber();
                result = resultNumbers.ReservationNumber;
                break;
        }

        await _repository.UpdateAsync(numbers[0], cancellationToken);

        return result;
    }
}