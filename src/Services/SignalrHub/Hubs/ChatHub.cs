using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactApi;
using drDotnet.Services.SignalrHub.Constants;
using drDotnet.Services.SignalrHub.Handler;
using drDotnet.Services.SignalrHub.MessageObjects;
using drDotnet.Services.SignalrHub.MessageObjects.Contact;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace drDotnet.Services.SignalrHub.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly Contact.ContactClient _contactClient;

        public ChatHub(Contact.ContactClient contactClient)
        {
            _contactClient = contactClient;
        }

        public async Task Update(MessageObjectBase msg)
        {
            switch (msg.Type)
            {
                case MessageDataType.GetContacts:
                    await new ContactHandler(Clients, Context, _contactClient).GetContacts(msg.Data);
                    break;
                default:
                    break;
            }
        }
    }
}