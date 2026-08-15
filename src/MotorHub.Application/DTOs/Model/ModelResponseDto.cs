namespace MotorHub.Application.DTOs.Model
{
    public class ModelResponseDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public Guid? BrandId { get; set; }
        public string Brand { get; set; }
    }
}
