using System.ComponentModel.DataAnnotations;

namespace MotorHub.Application.DTOs.Brand
{
    public class CreateBrandRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public required string Name { get; set; }
    }
}
