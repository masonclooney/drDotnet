using System.ComponentModel.DataAnnotations;

namespace drDotnet.Services.Contact.API.ViewModel
{
    public class ContactCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
    }
}