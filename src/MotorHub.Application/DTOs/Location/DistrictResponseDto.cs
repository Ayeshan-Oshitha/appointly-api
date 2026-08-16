namespace MotorHub.Application.DTOs.Location
{
    public class DistrictResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Province { get; set; } = null!;
        public Guid ProvinceId { get; set; }
    }
}
