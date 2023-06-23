using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;
public class SetRoomStateCheckInRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
}

public class SetRoomStateCheckInRequestValidator : CustomValidator<SetRoomStateCheckInRequest>
{
    public SetRoomStateCheckInRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public class SetRoomStateCheckInRequestHandler : IRequestHandler<SetRoomStateCheckInRequest, int>
{
    private readonly IRepositoryWithEvents<Room> _repository;
    public SetRoomStateCheckInRequestHandler(IRepositoryWithEvents<Room> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(SetRoomStateCheckInRequest request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(request.Id, cancellationToken);

        room.SetRoomStateCheckIn(false, true, true, false);
        room.DomainEvents.Add(EntityUpdatedEvent.WithEntity(room));
        await _repository.UpdateAsync(room, cancellationToken);

        return room.Id;
    }
}
