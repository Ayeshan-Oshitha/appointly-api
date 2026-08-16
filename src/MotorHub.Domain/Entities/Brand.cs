namespace MotorHub.Domain.Entities
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        // Navigation Property for related Models
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
