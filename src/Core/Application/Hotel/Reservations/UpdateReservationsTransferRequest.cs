namespace FSH.WebApi.Application.Hotel.Reservations;
public class UpdateReservationsTransferRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public string? Transfer { get; set; }
    public int GroupMasterId { get; set; }
}

public class UpdateReservationsTransferRequestValidator : AbstractValidator<UpdateReservationsTransferRequest>
{
    public UpdateReservationsTransferRequestValidator()
    {
        RuleFor(x => x.MandantId)
            .NotEmpty();
        RuleFor(x => x.Transfer) // Fraglich kein Transfer sollte nur bei Gruppenaustritt möglich sein
            .NotEmpty();
        RuleFor(x => x.GroupMasterId)
            .NotEmpty();
    }
}

public class UpdateReservationsTransferRequestHandler : IRequestHandler<UpdateReservationsTransferRequest, int>
{
    private readonly IDapperRepository _dapperRepository;

    public UpdateReservationsTransferRequestHandler(IDapperRepository dapperRepository)
    {
        _dapperRepository = dapperRepository;
    }

    public async Task<int> Handle(UpdateReservationsTransferRequest request, CancellationToken cancellationToken)
    {
        string sql = $"UPDATE lnx.Reservation SET Transfer = '{request.Transfer}' WHERE MandantId = {request.MandantId} AND GroupMasterId = {request.GroupMasterId}";
        int treffer = await _dapperRepository.ExecuteAsync<int>(sql, cancellationToken);
        return treffer;
    }
}
