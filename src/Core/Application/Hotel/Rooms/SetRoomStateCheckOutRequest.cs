using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;
public class SetRoomStateCheckOutRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
}

public class SetRoomStateCheckOutRequestValidator : CustomValidator<SetRoomStateCheckOutRequest>
{
    public SetRoomStateCheckOutRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public class SetRoomStateCheckOutRequestHandler : IRequestHandler<SetRoomStateCheckOutRequest, int>
{
    private readonly IRepositoryWithEvents<Room> _repository;

    public SetRoomStateCheckOutRequestHandler(IRepositoryWithEvents<Room> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(SetRoomStateCheckOutRequest request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(request.Id, cancellationToken);

        room.SetRoomStateCheckOut(false, false, true);
        room.DomainEvents.Add(EntityUpdatedEvent.WithEntity(room));
        await _repository.UpdateAsync(room, cancellationToken);

        return request.Id;
    }
}
