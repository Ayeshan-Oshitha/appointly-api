using System.ComponentModel.DataAnnotations;

namespace Appointly.Application.DTOs.Model
{
    public class CreateModelRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required Guid BrandId { get; set; }
    }
}
