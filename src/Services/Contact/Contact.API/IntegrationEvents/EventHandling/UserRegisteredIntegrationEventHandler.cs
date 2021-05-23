using System.Threading.Tasks;
using drDotnet.BuildingBlocks.EventBus.Abstractions;
using drDotnet.Services.Contact.API.Infrastructure;
using drDotnet.Services.Contact.API.IntegrationEvents.Events;
using drDotnet.Services.Contact.API.Model;
using Microsoft.Extensions.Logging;

namespace drDotnet.Services.Contact.API.IntegrationEvents.EventHandling
{
    public class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        private readonly ContactDbContext _contactContext;
        private readonly ILogger<UserRegisteredIntegrationEventHandler> _logger;

        public UserRegisteredIntegrationEventHandler(ContactDbContext contactContext, ILogger<UserRegisteredIntegrationEventHandler> logger)
        {
            _contactContext = contactContext;
            _logger = logger;
        }

        public async Task Handle(UserRegisteredIntegrationEvent @event)
        {
            _logger.LogInformation("start creating user in contact service", @event);
            var user = new User { Id = @event.UserId, Email = @event.Name, Name = @event.Name };
            _contactContext.Users.Add(user);
            await _contactContext.SaveChangesAsync();
        }
    }
}