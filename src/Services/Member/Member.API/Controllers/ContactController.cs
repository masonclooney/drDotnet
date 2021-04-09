using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using drDotnet.Services.Member.API.Infrastructure;
using drDotnet.Services.Member.API.Model;
using drDotnet.Services.Member.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace drDotnet.Services.Member.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly MemberContext _memberContext;

        public ContactController(MemberContext memberContext)
        {
            _memberContext = memberContext ?? throw new ArgumentNullException(nameof(memberContext));

            memberContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ContactViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ContactViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _memberContext.Contacts
                .LongCountAsync();

            var itemsOnPage = await _memberContext.Contacts
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .Select(c => new ContactViewModel {
                    Email =  c.User.Email,
                    Name = c.Name,
                    UserId = c.Id
                }).ToListAsync();

            var model = new PaginatedItemsViewModel<ContactViewModel>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet]
        [Route("items/{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ContactViewModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ContactViewModel>> ItemByIdAsync(long id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var item = await _memberContext.Contacts.Include(c => c.User)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                return new ContactViewModel
                {
                    Email = item.User.Email,
                    UserId = item.User.Id,
                    Name = item.User.Name
                };
            }
            return NotFound();
        }

        [HttpPost]
        [Route("items")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateContactAsync([FromBody] User model)
        {
            var user = await _memberContext.Users
                .Where(u => u.Email == model.Email).SingleOrDefaultAsync();

            if (user == null)
                return NotFound();

            var contact = new Contact
            {
                Name = model.Name,
                UserId = user.Id,
                OwnerId = 1
            };

            _memberContext.Contacts.Add(contact);
            await _memberContext.SaveChangesAsync();

            Console.WriteLine(nameof(ItemByIdAsync));
            
            return CreatedAtAction("items", new { id = contact.Id }, null);
        }
    }   
}