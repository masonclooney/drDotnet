using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using drDotnet.Services.Contact.API.Infrastructure;
using drDotnet.Services.Contact.API.Model;
using drDotnet.Services.Contact.API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace drDotnet.Services.Contact.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ContactDbContext _contactContext;
        public ContactController(ContactDbContext contactContext)
        {
            _contactContext = contactContext ?? throw new ArgumentException(nameof(contactContext));

            contactContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteContactAsync(long id)
        {
            var ownerId = Int64.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var contact = _contactContext.Contacts
                .Where(c => c.OwnerId == ownerId && c.UserId == id).SingleOrDefault();

            if (contact == null)
            {
                return NotFound();
            }

            _contactContext.Contacts.Remove(contact);
            await _contactContext.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<User>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<User>>> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var ownerId = Int64.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var totalItems = await _contactContext.Contacts.Where(c => c.OwnerId == ownerId).LongCountAsync();

            var itemsOnPage = await _contactContext.Contacts.Where(c => c.OwnerId == ownerId).OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex).Take(pageSize).Select(c => c.User).ToListAsync();

            return new PaginatedItemsViewModel<User>(pageIndex, pageSize, totalItems, itemsOnPage);
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateContactAsync([FromBody] ContactCreateDto model)
        {
            var ownerId = Int64.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _contactContext.Users
                .Where(u => u.Email == model.Email).SingleOrDefaultAsync();

            if (user == null) 
                return NotFound();

            var contact = await _contactContext.Contacts
                .Where(c => c.OwnerId == ownerId && c.UserId == user.Id).SingleOrDefaultAsync();

            if (contact != null)
            {
                contact.Name = model.Name;
                await _contactContext.SaveChangesAsync();
                return Ok(contact);
            }
            contact = new Model.Contact
            {
                Name = model.Name,
                UserId = user.Id,
                OwnerId = ownerId
            };

            _contactContext.Contacts.Add(contact);
            await _contactContext.SaveChangesAsync();

            return Created("", contact);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> ItemByIdAsync(long id)
        {   
            var ownerId = Int64.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var contact = await _contactContext.Contacts
                .Where(c => c.OwnerId == ownerId && c.UserId == id)
                .Select(c => c.User).SingleOrDefaultAsync();

            if (contact == null) return NotFound();

            return Ok(contact);
        }

    }
}