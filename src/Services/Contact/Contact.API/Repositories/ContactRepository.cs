using System.Collections.Generic;
using System.Linq;
using drDotnet.Services.Contact.API.Models;

namespace drDotnet.Services.Contact.API.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private static readonly ICollection<ContactModel> Contacts =
            new List<ContactModel>
            {
                new ContactModel { Id = 977588813, Name = "", OwnerId = 977588814, Mutual = false },
                new ContactModel { Id = 977588812, Name = "", OwnerId = 977588814, Mutual = false },
                new ContactModel { Id = 977588811, Name = "", OwnerId = 977588814, Mutual = false },
                new ContactModel { Id = 977588810, Name = "", OwnerId = 977588814, Mutual = false },
                new ContactModel { Id = 977588809, Name = "", OwnerId = 977588814, Mutual = false },
                new ContactModel { Id = 977588808, Name = "", OwnerId = 977588814, Mutual = false },
                new ContactModel { Id = 977588807, Name = "", OwnerId = 977588814, Mutual = false },
                new ContactModel { Id = 977588806, Name = "", OwnerId = 977588814, Mutual = false },
                new ContactModel { Id = 977588805, Name = "", OwnerId = 977588814, Mutual = false },
            };
        public ContactModel CreateContact(ContactModel model)
        {
            Contacts.Add(model);
            return model;
        }

        public List<ContactModel> GetContacts(long id)
        {
            return Contacts.Where(x => x.OwnerId == id).ToList();
        }
    }
}