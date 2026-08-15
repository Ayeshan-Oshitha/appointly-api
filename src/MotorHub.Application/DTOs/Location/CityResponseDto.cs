namespace MotorHub.Application.DTOs.Location
{
    public class CityResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
    }
}
