using System.ComponentModel.DataAnnotations;

namespace Appointly.Application.DTOs.Location
{
    public class UpdateCityRequestDto
    {
        [MinLength(1)]
        [MaxLength(100)]
        public string? Name { get; set; }

        public Guid? ProvinceId { get; set; }

        public Guid? DistrictId { get; set; }
    }
}
