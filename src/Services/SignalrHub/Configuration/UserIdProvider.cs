using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace drDotnet.Services.SignalrHub.Configuration
{
    public class UserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}