using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.RoomReservations;
public class CreateRoomReservationRequest : IRequest<int>
{
    public int MandantId { get; set; }
    public int RoomId { get; set; }
    public string? Name { get; set; }
    // public DateTime Occupied { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public int ReservationId { get; set; }
}

public class CreateRoomReservationRequestValidator : CustomValidator<CreateRoomReservationRequest>
{
    public CreateRoomReservationRequestValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(50)
        .WithMessage("RoomName not set");
    }
}

public class CreateRoomReservationRequestHandler : IRequestHandler<CreateRoomReservationRequest, int>
{
    private readonly IRepository<RoomReservation> _repository;

    public CreateRoomReservationRequestHandler(IRepository<RoomReservation> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateRoomReservationRequest request, CancellationToken cancellationToken)
    {
        int days = (request.Departure.Date - request.Arrival.Date).Days;
        int counter = 0;
        for (int i = 0; i < days; i++)
        {
            counter++;
            DateTime workDate = request.Arrival.Date.AddDays(i);
            var roomReservation = new RoomReservation(request.MandantId, request.RoomId, request.Name, workDate, request.ReservationId);
            await _repository.AddAsync(roomReservation, cancellationToken);
        }

        return counter;
    }
}