using FSH.WebApi.Domain.Accounting;
using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Accounting.Merchandises.EventHandlers;

public class MerchandiseCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<Merchandise>>
{
    private readonly ILogger<MerchandiseCreatedEventHandler> _logger;

    public MerchandiseCreatedEventHandler(ILogger<MerchandiseCreatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Merchandise> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}

// public class MerchandiseCreatedEventHandler : INotificationHandler<EventNotification<EntityCreatedEvent<Merchandise>>>
// {
//    private readonly ILogger<MerchandiseCreatedEventHandler> _logger;
//    public MerchandiseCreatedEventHandler(ILogger<MerchandiseCreatedEventHandler> logger)
//    {
//        _logger = logger;
//    }

//    Task INotificationHandler<EventNotification<EntityCreatedEvent<Merchandise>>>.Handle(EventNotification<EntityCreatedEvent<Merchandise>> notification, CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("{event} Triggered", notification.DomainEvent.GetType().Name);
//        return Task.CompletedTask;
//    }
// }
