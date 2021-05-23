using System;
using System.Threading.Tasks;
using drDotnet.BuildingBlocks.EventBus.Abstractions;
using drDotnet.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace drDotnet.Services.Identity.API.IntegrationEvents
{
    public class IdentityIntegrationEventService : IIdentityIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<IdentityIntegrationEventService> _logger;

        public IdentityIntegrationEventService(IEventBus eventBus, ILogger<IdentityIntegrationEventService> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
                 _eventBus.Publish(evt);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
            }
        }
    }
}