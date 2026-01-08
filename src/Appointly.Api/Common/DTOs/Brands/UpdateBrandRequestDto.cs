using System.ComponentModel.DataAnnotations;

namespace Appointly.Api.Common.DTOs.Brands
{
    public class UpdateBrandRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(200)]
        public required string Name { get; set; }
    }
}
