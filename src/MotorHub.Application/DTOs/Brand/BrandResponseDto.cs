namespace MotorHub.Application.DTOs.Brand
{
    public class BrandResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}
