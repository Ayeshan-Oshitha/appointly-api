namespace Appointly.Domain.Entities
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        // Foreign key to Province
        public Guid ProvinceId { get; set; }
        // Navigation Property to Province
        public Province Province { get; set; }

        // Foreign key to District
        public Guid DistrictId { get; set; }
        // Navigation Property to District
        public District District { get; set; } 
    }
}
