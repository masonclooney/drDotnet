namespace drDotnet.Services.Member.API.Model
{
    public class Contact
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long OwnerId { get; set; }
        public User Owner { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public bool MutualContact { get; set; }
    }
}