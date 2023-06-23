﻿using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms;
public class SetRoomStateArrivalExpectedRequest : IRequest<int>
{
    public int Id { get; set; }
    public int MandantId { get; set; }
}

public class SetRoomStateArrivalExpectedRequestValidator : CustomValidator<SetRoomStateArrivalExpectedRequest>
{
    public SetRoomStateArrivalExpectedRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public class SetRoomStateArrivalExpectedRequestHandler : IRequestHandler<SetRoomStateArrivalExpectedRequest, int>
{
    private readonly IRepositoryWithEvents<Room> _repository;
    public SetRoomStateArrivalExpectedRequestHandler(IRepositoryWithEvents<Room> repository) =>
                             (_repository) = (repository);
    public async Task<int> Handle(SetRoomStateArrivalExpectedRequest request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(request.Id, cancellationToken);
        room.SetRoomStateArrivalExpected();
        room.DomainEvents.Add(EntityUpdatedEvent.WithEntity(room));
        await _repository.UpdateAsync(room, cancellationToken);
        return request.Id;
    }
}
