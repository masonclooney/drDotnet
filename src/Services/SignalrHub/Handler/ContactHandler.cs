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
        private readonly string AccessToken;

        public ContactHandler(IHubCallerClients clients, HubCallerContext context, Contact.ContactClient contactClient)
        {
            Clients = clients;
            Context = context;
            _contactClient = contactClient;
            UserIdentifier = Int64.Parse(Context.UserIdentifier);
            AccessToken = Context.GetHttpContext().Request.Query["access_token"];
        }

        public async Task CreateContact(string msg)
        {
            var data = JsonSerializer.Deserialize<CreateContactMsgObj>(msg);
            var request = ContactCreateRequest(data);

            var authHeader = GetAuthorizationHeader();

            var result = await _contactClient.CreateContactAsync(request, authHeader);

            await SendBack(MakeObjBase(MessageDataType.updateCreateContact, result));

            var contactUser = await _contactClient
                .GetContactAsync(ContactItemRequest(result.UserId), authHeader);

            await SendBack(MakeObjBase(MessageDataType.UpdateUser, MapToUpdateUser(contactUser)));
        }

        public async Task GetContacts(string msg)
        {
            var data = JsonSerializer.Deserialize<GetContactsMsgObj>(msg);
            var request = ContactItemsRequest(data);

            var result = await _contactClient.GetContactsAsync(request, GetAuthorizationHeader());
            List<long> userIds = new List<long>();
            
            foreach(var contact in result.Data)
            {
                userIds.Add(contact.Id);
                var user = MapToUpdateUser(contact);
                await SendBack(MakeObjBase(MessageDataType.UpdateUser, user));
            }

            var updateContact = MapToUpdateContacts(userIds, result);
            await SendBack(MakeObjBase(MessageDataType.UpdateContact, updateContact));
        }

        private Metadata GetAuthorizationHeader()
        {
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {AccessToken}");
            return headers;
        }

        private ContactItemsRequest ContactItemsRequest(GetContactsMsgObj data)
        {
            return new ContactItemsRequest { PageIndex = data.PageIndex, PageSize = data.PageSize };
        }

        private ContactCreateRequest ContactCreateRequest(CreateContactMsgObj data)
        {
            return new ContactCreateRequest { Name = data.Name, Email = data.Email };
        }

        private ContactItemRequest ContactItemRequest(long id)
        {
            return new ContactItemRequest { Id = id };
        }

        private MessageObjectBase MakeObjBase(string type, object data)
        {
            return new MessageObjectBase
            {
                Type = type,
                Data = JsonSerializer.Serialize(data)
            };
        }

        private async Task SendBack(MessageObjectBase data)
        {
            await Clients.Caller.SendAsync("update", data);
        }

        private UpdateContacts MapToUpdateContacts(List<long> ids, PaginatedContactResponse con)
        {
            return new UpdateContacts
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