using System.ComponentModel.DataAnnotations;

namespace Appointly.Application.DTOs.Authentication
{
    public class RegisterRequestDto
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }
    }
}
