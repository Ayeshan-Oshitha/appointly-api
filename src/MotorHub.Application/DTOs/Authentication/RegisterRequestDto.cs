using System.ComponentModel.DataAnnotations;

namespace MotorHub.Application.DTOs.Authentication
{
    public class RegisterRequestDto
    {
        // Lengths mirror UserConfiguration; without them an over-long name reaches the database
        // and fails as a 500 rather than a 400.
        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public required string PhoneNumber { get; set; }
    }
}
