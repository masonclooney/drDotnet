using System.Collections.Generic;
using drDotnet.Services.Contact.API.Models;

namespace drDotnet.Services.Contact.API.Repositories
{
    public interface IContactRepository
    {
        ContactModel CreateContact(ContactModel model);

        List<ContactModel> GetContacts(long id);
    }
}