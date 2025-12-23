namespace Appointly.Domain.Entities
{
    public class Province
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        // Navigation property for related Districts
        public ICollection<District> Districts { get; set; } = new List<District>();
        // Navigation property for related Cities
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
