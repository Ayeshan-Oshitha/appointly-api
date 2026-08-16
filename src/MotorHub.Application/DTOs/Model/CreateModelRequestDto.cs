using System.ComponentModel.DataAnnotations;

namespace MotorHub.Application.DTOs.Model
{
    public class CreateModelRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(200)]
        public required string Name { get; set; }

        [Required]
        public required Guid BrandId { get; set; }
    }
}
