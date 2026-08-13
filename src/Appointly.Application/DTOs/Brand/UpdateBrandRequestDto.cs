using System.ComponentModel.DataAnnotations;

namespace Appointly.Application.DTOs.Brand
{
    public class UpdateBrandRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(200)]
        public required string Name { get; set; }
    }
}
