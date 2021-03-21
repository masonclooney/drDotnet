using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
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

        public async Task CreateContact()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:9001");
            var client = new Contact.ContactClient(channel);
            var reply = await client.CreateContactAsync(new ContactRequest
            {
                Email = "asdf@gmail.com",
                Name = "meys",
                Id = "4ac1ec1f-5329-4bab-8e7b-7de4aa830d6f"
            });

            await Clients.Caller.SendAsync("ContactCreated", reply);
        }

        public async IAsyncEnumerable<ContactRequest> GetContacts()
        {
             using var channel = GrpcChannel.ForAddress("https://localhost:9001");
            var client = new Contact.ContactClient(channel);
            var reply = client.GetContacts(new ContactRequest
            {
                Email = "asdf@gmail.com",
                Name = "meys",
                Id = "4ac1ec1f-5329-4bab-8e7b-7de4aa830d6f"
            });

            while (await reply.ResponseStream.MoveNext())
            {
                yield return reply.ResponseStream.Current;
            }
        }
    }
}