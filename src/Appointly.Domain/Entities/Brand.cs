namespace Appointly.Domain.Entities
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        // Navigation Property for related Models
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
