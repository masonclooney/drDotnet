using System;
using System.Threading.Tasks;
using ContactApi;
using drDotnet.Services.Contact.API.Infrastructure;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using static ContactApi.Contact;

namespace drDotnet.Services.Contact.API.Grpc
{
    public class ContactService : ContactBase
    {
        private readonly ContactContext _contactContext;
        private readonly ILogger _logger;

        public ContactService(ContactContext contactContext, ILogger logger)
        {
            _contactContext = contactContext ?? throw new ArgumentNullException(nameof(contactContext));
            _logger = logger;
        }

        public override Task<ContactCreateResponse> CreateContact(ContactCreateRequest request, ServerCallContext context)
        {
            return base.CreateContact(request, context);
        }

        public override Task<Response> DeleteContact(ContactItemRequest request, ServerCallContext context)
        {
            return base.DeleteContact(request, context);
        }

        public override Task<PaginatedContactResponse> GetContacts(ContactItemsRequest request, ServerCallContext context)
        {
            return base.GetContacts(request, context);
        }
    }
}