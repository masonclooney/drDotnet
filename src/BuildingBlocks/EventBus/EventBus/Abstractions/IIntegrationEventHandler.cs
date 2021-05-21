using System.Threading.Tasks;
using drDotnet.BuildingBlocks.EventBus.Events;

namespace drDotnet.BuildingBlocks.EventBus.Abstractions
{
    public interface IIntegrationEventHandler
    {
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}