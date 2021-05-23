using System.Threading.Tasks;
using drDotnet.BuildingBlocks.EventBus.Events;

namespace drDotnet.Services.Identity.API.IntegrationEvents
{
    public interface IIdentityIntegrationEventService
    {
        void PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}