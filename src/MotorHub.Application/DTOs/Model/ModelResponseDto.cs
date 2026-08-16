namespace MotorHub.Application.DTOs.Model
{
    public class ModelResponseDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public Guid? BrandId { get; set; }
        public string Brand { get; set; } = null!;
    }
}
