namespace drDotnet.Services.Contact.API.Model
{
    public class Contact
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long OwnerId { get; set; }
        public User Owner { get; set; }
        public string Name { get; set; }
    }
}