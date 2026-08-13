using System.ComponentModel.DataAnnotations;

namespace Appointly.Application.DTOs.Model
{
    public class UpdateModelRequestDto
    {
        [MinLength(1)]
        [MaxLength(200)]
        public string? Name { get; set; }
        public Guid? BrandId { get; set; }
    }
}
