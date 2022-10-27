using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.PluGroups.EventHandlers;

public class PluGroupUpdatedEventHandler : EventNotificationHandler<EntityUpdatedEvent<PluGroup>>
{
    private readonly ILogger<PluGroupUpdatedEventHandler> _logger;

    public PluGroupUpdatedEventHandler(ILogger<PluGroupUpdatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityUpdatedEvent<PluGroup> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}

// public class PluGroupUpdatedEventHandler : INotificationHandler<EventNotification<EntityUpdatedEvent<PluGroup>>>
// {
//    private readonly ILogger<PluGroupUpdatedEventHandler> _logger;
//    public PluGroupUpdatedEventHandler(ILogger<PluGroupUpdatedEventHandler> logger)
//    {
//        _logger = logger;
//    }
//    Task INotificationHandler<EventNotification<EntityUpdatedEvent<PluGroup>>>.Handle(EventNotification<EntityUpdatedEvent<PluGroup>> notification, CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("{event} Triggered", notification.DomainEvent.GetType().Name);
//        return Task.CompletedTask;
//    }
// }