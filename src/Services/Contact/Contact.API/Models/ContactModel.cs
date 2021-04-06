namespace drDotnet.Services.Contact.API.Models
{
    public class ContactModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long OwnerId { get; set; }

        public bool Mutual { get; set; }
    }
}