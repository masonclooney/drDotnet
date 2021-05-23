using drDotnet.BuildingBlocks.EventBus.Events;

namespace drDotnet.Services.Identity.API.IntegrationEvents.Events
{
    public class UserRegisteredIntegrationEvent : IntegrationEvent
    {
        public long UserId { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public UserRegisteredIntegrationEvent(long userId, string name, string email)
        {
            UserId = userId;
            Name = name;
            Email = email;
        }
    }
}