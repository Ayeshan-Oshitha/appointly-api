using System.ComponentModel.DataAnnotations;

namespace MotorHub.Application.DTOs.Location
{
    public class UpdateCityRequestDto
    {
        [MinLength(1)]
        [MaxLength(200)]
        public string? Name { get; set; }

        public Guid? ProvinceId { get; set; }

        public Guid? DistrictId { get; set; }
    }
}
