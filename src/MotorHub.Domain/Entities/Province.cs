namespace MotorHub.Domain.Entities
{
    public class Province
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        // Navigation property for related Districts
        public ICollection<District> Districts { get; set; } = new List<District>();
        // Navigation property for related Cities
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
