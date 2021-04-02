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
            new Dictionary<string, ContactRequest>
            {
                {"123", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"223", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"323", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"423", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"523", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"623", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"723", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"823", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"923", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"133", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
                {"134", new ContactRequest { Name = "meysam", Email = "kholoosim1@gmail.com", Id = "qwjr4lkst" }},
            };

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