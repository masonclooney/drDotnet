using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using drDotnet.Services.Contact.API.Infrastructure;
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
        private readonly ContactContext _contactContext;
        public ContactController(ContactContext contactContext)
        {
            _contactContext = contactContext ?? throw new ArgumentException(nameof(contactContext));

            contactContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateContactAsync([FromBody] ContactCreateDto model)
        {
            var ownerId = 1;
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

    }
}