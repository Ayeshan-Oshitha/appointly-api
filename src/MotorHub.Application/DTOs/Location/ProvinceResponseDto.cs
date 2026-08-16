namespace MotorHub.Application.DTOs.Location
{
    public class ProvinceResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}
