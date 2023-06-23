using FSH.WebApi.Domain.Common.Events;
using FSH.WebApi.Domain.Hotel;

namespace FSH.WebApi.Application.Hotel.Rooms.EventHandlers;
public class RoomUpdateEventHandler : EventNotificationHandler<EntityUpdatedEvent<Room>>
{
    private readonly ILogger<RoomUpdateEventHandler> _logger;

    public RoomUpdateEventHandler(ILogger<RoomUpdateEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityUpdatedEvent<Room> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{@event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}
