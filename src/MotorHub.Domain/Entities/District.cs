namespace MotorHub.Domain.Entities
{
    public class District
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        // Foreign key to Province
        public Guid ProvinceId { get; set; }

        // Navigation Property to Province
        public Province Province { get; set; } 

        // Navigation Property for related Cities
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
