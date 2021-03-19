using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace drDotnet.Services.SignalrHub.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.UserIdentifier, message);
        }

        public async Task Update(string id, string msg)
        {
            await Clients.User(id).SendAsync("update", msg);
        }
    }
}