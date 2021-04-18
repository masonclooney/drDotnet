using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ContactApi;
using drDotnet.Services.SignalrHub.Constants;
using drDotnet.Services.SignalrHub.MessageObjects;
using drDotnet.Services.SignalrHub.MessageObjects.Contact;
using drDotnet.Services.SignalrHub.MessageObjects.User;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR;

namespace drDotnet.Services.SignalrHub.Handler
{
    public class ContactHandler
    {
        private IHubCallerClients Clients { get; set; }
        private  HubCallerContext Context { get; set; }
        private readonly Contact.ContactClient _contactClient;
        private readonly long UserIdentifier;

        public ContactHandler(IHubCallerClients clients, HubCallerContext context, Contact.ContactClient contactClient)
        {
            Clients = clients;
            Context = context;
            _contactClient = contactClient;
            UserIdentifier = Int64.Parse(Context.UserIdentifier);
        }

        public async Task GetContacts(string msg)
        {
            var data = JsonSerializer.Deserialize<GetContactsMsgObj>(msg);
            
            var request = new ContactItemsRequest { PageIndex = data.PageIndex, PageSize = data.PageSize };

            var accessToken = Context.GetHttpContext().Request.Query["access_token"];
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {accessToken}");

            var result = await _contactClient.GetContactsAsync(request, headers);
            List<long> userIds = new List<long>();

            foreach(var contact in result.Data)
            {
                userIds.Add(contact.Id);
                var user = MapToUpdateUser(contact);
                await Clients.Caller.SendAsync("update", user);
            }

            var updateContact = MapToUpdateContact(userIds, result);

            var sendData = new MessageObjectBase
            {
                Type = MessageDataType.UpdateContact,
                Data = JsonSerializer.Serialize(updateContact)
            };

            await Clients.Caller.SendAsync("update", sendData);
        }

        private UpdateContact MapToUpdateContact(List<long> ids, PaginatedContactResponse con)
        {
            return new UpdateContact
            {
                Ids = ids,
                PageIndex = con.PageIndex,
                PageSize = con.PageSize
            };
        }

        private UpdateUser MapToUpdateUser(ContactItemResponse contact) 
        {
            return new UpdateUser()
                {
                    Email = contact.Email,
                    Name = contact.Name,
                    Id = contact.Id
                };
        }
    }
}