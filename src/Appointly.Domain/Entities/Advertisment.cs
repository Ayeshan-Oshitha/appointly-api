using Appointly.Domain.Common;

namespace Appointly.Domain.Entities
{
    public class Advertisment
    {
        public Guid Id { get; set; }
        public string Title { get; set; }  = null!;
        public string Description { get; set; } = null!;
        public int Year { get; set; } 
        public decimal Price { get; set; } 
        public int? EngineCapacity { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public VehicleCondition VehicleCondition { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Address { get; set; }
        public Guid BrandId { get; set; }
        public Brand? Brand { get; set; }
        public Guid ModelId { get; set; }
        public Model? Model { get; set; }
        public Guid CityId { get; set; }
        public City? City { get; set; }

        public string ContactName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public bool IsHidePhone { get; set; } = false;
        public bool? IsWhatsapp { get; set; }

        //public Guid SellerId { get; set; }
        //public Guid AdminId { get; set; }
        // Add Navigation Property

        public bool IsBiddable { get; set; } = false;
        public AdStatus Status { get; set; } = AdStatus.Pending;
        public bool IsDeleted { get; set; } = false;
        public string? RejectedReason { get; set; }
    }
}
