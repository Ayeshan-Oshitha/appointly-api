using System.ComponentModel.DataAnnotations;

namespace Appointly.Api.Common.DTOs.Brands
{
    public class CreateBrandRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public required string Name { get; set; }
    }
}
