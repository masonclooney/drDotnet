using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactApi;
using drDotnet.Services.Contact.API.Infrastructure;
using drDotnet.Services.Contact.API.Model;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
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

        public override async Task<ContactItemResponse> GetContact(ContactItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call ContactService.GetContact");
            var ownerId = Int64.Parse(context.GetHttpContext().User
                .FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);

            var contact = await _contactContext.Contacts
                .Where(c => c.OwnerId == ownerId && c.UserId == request.Id)
                .Select(c => c.User).SingleOrDefaultAsync();

            if (contact == null)
            {
                context.Status = new Status(StatusCode.NotFound, "Contact not found");
                return null;
            }

            context.Status = new Status(StatusCode.OK, string.Empty);
            return new ContactItemResponse() { Id = contact.Id, Email = contact.Email, Name = contact.Name };
        }

        public override async Task<ContactCreateResponse> CreateContact(ContactCreateRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call ContactService.CreateContact");
            var ownerId = Int64.Parse(context.GetHttpContext().User
                .FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);

            var user = await _contactContext.Users
                .Where(u => u.Email == request.Email).SingleOrDefaultAsync();
            
            if (user == null)
            {
                context.Status = new Status(StatusCode.NotFound, "User do not exist");
                return null;
            }
            
            var contact = await _contactContext.Contacts
                .Where(c => c.OwnerId == ownerId && c.UserId == user.Id).SingleOrDefaultAsync();

            if (contact != null)
            {
                contact.Name = request.Name;
                await _contactContext.SaveChangesAsync();
                context.Status = new Status(StatusCode.OK, "Contact updated!");
                return new ContactCreateResponse() { Name = contact.Name, OwnerId = ownerId, UserId = contact.UserId};
            }

            contact = new Model.Contact
            {
                Name = request.Name,
                UserId = user.Id,
                OwnerId = ownerId
            };

            _contactContext.Contacts.Add(contact);
            await _contactContext.SaveChangesAsync();

            context.Status = new Status(StatusCode.OK, "Contact created");
            return new ContactCreateResponse() { Name = contact.Name, OwnerId = ownerId, UserId = contact.UserId};
        }

        public override async Task<Response> DeleteContact(ContactItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call ContactService.DeleteContact");
            var ownerId = Int64.Parse(context.GetHttpContext().User
                .FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);

            var contact = _contactContext.Contacts
                .Where(c => c.OwnerId == ownerId && c.UserId == request.Id).SingleOrDefault();

            if (contact == null)
            {
                context.Status = new Status(StatusCode.NotFound, "Contact not found");
                return null;
            }

            _contactContext.Contacts.Remove(contact);
            await _contactContext.SaveChangesAsync();

            context.Status = new Status(StatusCode.OK, "Contact deleted");
            return null;
        }

        public override async Task<PaginatedContactResponse> GetContacts(ContactItemsRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call ContactService.GetContacts");
            var ownerId = Int64.Parse(context.GetHttpContext().User
                .FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);

            var totalItems = await _contactContext.Contacts.Where(c => c.OwnerId == ownerId).LongCountAsync();

            var itemsOnPage = await _contactContext.Contacts.Where(c => c.OwnerId == ownerId).OrderBy(c => c.Name)
                .Skip(request.PageSize * request.PageIndex).Take(request.PageSize).Select(c => c.User).ToListAsync();

            var model = this.MapToResponse(itemsOnPage, totalItems, request.PageIndex, request.PageSize);
            context.Status = new Status(StatusCode.OK, string.Empty);

            return model;
        }

        private PaginatedContactResponse MapToResponse(List<User> items, long count, int pageIndex, int pageSize)
        {
            var result = new PaginatedContactResponse()
            {
                Count = count,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            items.ForEach(i =>
            {
                result.Data.Add(new ContactItemResponse
                {
                    Id = i.Id,
                    Email = i.Email,
                    Name = i.Name
                });
            });

            return result;
        }
    }
}