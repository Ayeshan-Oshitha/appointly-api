using System.ComponentModel.DataAnnotations;

namespace Appointly.Api.Common.DTOs.Location
{
    public class AddCityRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required Guid ProvinceId { get; set; }

        [Required]
        public required Guid DistrictId { get; set; }

    }
}
