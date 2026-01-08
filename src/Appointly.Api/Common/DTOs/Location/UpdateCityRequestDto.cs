using System.ComponentModel.DataAnnotations;

namespace Appointly.Api.Common.DTOs.Location
{
    public class UpdateCityRequestDto
    {
        [MinLength(1)]
        [MaxLength(100)]
        public required string Name { get; set; }

        public required Guid ProvinceId { get; set; }

        public required Guid DistrictId { get; set; }
    }
}
