
using System.ComponentModel.DataAnnotations;

namespace Bookmanagementapi.Models.DTO.Requests
{
    public class UserRegistrationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}