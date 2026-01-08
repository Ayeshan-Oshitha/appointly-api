using Appointly.Domain.Common.Enum;
using Appointly.Domain.Entities;

namespace Appointly.Application.Services.Advertisment.Contracts
{
    public class CreateAdvertisementRequest
    {
        public string  Title { get; set; } 
        public string Description { get; set; } 
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int? EngineCapacity { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public VehicleCondition VehicleCondition { get; set; }
        public string? Address { get; set; }
        public Guid BrandId { get; set; }
        public Guid ModelId { get; set; }
        public Guid CityId { get; set; }
        public string ContactName { get; set; } 
        public string ContactPhone { get; set; } 
        public string ContactEmail { get; set; } 
        public bool IsHidePhone { get; set; }
        public bool? IsWhatsapp { get; set; }
        public bool IsBiddable { get; set; }
    }
}
