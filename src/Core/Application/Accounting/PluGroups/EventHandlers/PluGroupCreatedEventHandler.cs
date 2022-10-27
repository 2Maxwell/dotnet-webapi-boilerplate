using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.PluGroups.EventHandlers;

public class PluGroupCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<PluGroup>>
{
    private readonly ILogger<PluGroupCreatedEventHandler> _logger;

    public PluGroupCreatedEventHandler(ILogger<PluGroupCreatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<PluGroup> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}

// public class PluGroupCreatedEventHandler : INotificationHandler<EventNotification<EntityCreatedEvent<PluGroup>>>
// {
//    private readonly ILogger<PluGroupCreatedEventHandler> _logger;
//    public PluGroupCreatedEventHandler(ILogger<PluGroupCreatedEventHandler> logger)
//    {
//        _logger = logger;
//    }
//    Task INotificationHandler<EventNotification<EntityCreatedEvent<PluGroup>>>.Handle(EventNotification<EntityCreatedEvent<PluGroup>> notification, CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("{event} Triggered", notification.DomainEvent.GetType().Name);
//        return Task.CompletedTask;
//    }
// }
