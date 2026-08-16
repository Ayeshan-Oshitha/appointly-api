namespace MotorHub.Application.DTOs.Location
{
    public class CityResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string District { get; set; } = null!;
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
    }
}
