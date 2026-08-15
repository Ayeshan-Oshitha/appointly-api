namespace MotorHub.Domain.Entities
{
    public class Model
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        // Foreign key to Brand
        public Guid BrandId { get; set; }
        // Navigation Property to Brand
        public Brand Brand { get; set; }
    }
}
