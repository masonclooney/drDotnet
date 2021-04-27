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
using Microsoft.Extensions.Logging;

namespace drDotnet.Services.SignalrHub.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly Contact.ContactClient _contactClient;
        private readonly ILogger _logger;

        public ChatHub(Contact.ContactClient contactClient, ILogger<ChatHub> logger)
        {
            _contactClient = contactClient;
            _logger = logger;
        }

        public async Task Update(MessageObjectBase msg)
        {
            switch (msg.Type)
            {
                case MessageDataType.GetContacts:
                    await new ContactHandler(Clients, Context, _contactClient).GetContacts(msg.Data);
                    break;
                case MessageDataType.CreateContact:
                    await new ContactHandler(Clients, Context, _contactClient).CreateContact(msg.Data);
                    break;
                default:
                    break;
            }
        }
    }
}