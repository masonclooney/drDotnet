using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace drDotnet.Services.Contact.API.rpcServices
{
    public class ContactService : Contact.ContactBase
    {
        private static readonly IDictionary<string, ContactRequest> Contacts =
            new Dictionary<string, ContactRequest>();

        private readonly ILogger<ContactService> _logger;
        public ContactService(ILogger<ContactService> logger)
        {
            _logger = logger;
        }

        public override Task<ContactReply> CreateContact(ContactRequest request, ServerCallContext context)
        {
            Contacts.Add(Guid.NewGuid().ToString(), request);
            var response = new ContactReply{ Message = "added" };
            return Task.FromResult(response);
        }

        public override async Task GetContacts(ContactRequest request,
            IServerStreamWriter<ContactRequest> responseStream, ServerCallContext context)
        {
            foreach (var value in Contacts.Values)
            {
                await responseStream.WriteAsync(value);
            }
        }
    }
}