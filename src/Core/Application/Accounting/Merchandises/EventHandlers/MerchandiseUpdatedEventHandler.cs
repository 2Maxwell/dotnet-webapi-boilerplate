using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.Merchandises.EventHandlers;

public class MerchandiseUpdatedEventHandler : EventNotificationHandler<EntityUpdatedEvent<Merchandise>>
{
    private readonly ILogger<MerchandiseUpdatedEventHandler> _logger;

    public MerchandiseUpdatedEventHandler(ILogger<MerchandiseUpdatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityUpdatedEvent<Merchandise> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}

// public class MerchandiseUpdatedEventHandler : INotificationHandler<EventNotification<EntityUpdatedEvent<Merchandise>>>
// {
//    private readonly ILogger<MerchandiseUpdatedEventHandler> _logger;
//    public MerchandiseUpdatedEventHandler(ILogger<MerchandiseUpdatedEventHandler> logger)
//    {
//        _logger = logger;
//    }
//    Task INotificationHandler<EventNotification<EntityUpdatedEvent<Merchandise>>>.Handle(EventNotification<EntityUpdatedEvent<Merchandise>> notification, CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("{event} Triggered", notification.DomainEvent.GetType().Name);
//        return Task.CompletedTask;
//    }
// }