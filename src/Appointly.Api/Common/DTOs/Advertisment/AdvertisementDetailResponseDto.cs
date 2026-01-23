namespace Appointly.Api.Common.DTOs.Advertisment
{
    public class AdvertisementDetailResponseDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; } 
        public string? Description { get; set; } 
        public int? Year { get; set; }
        public decimal? Price { get; set; }
        public int? EngineCapacity { get; set; }
        public string? FuelType { get; set; }
        public string? TransmissionType { get; set; }
        public string? VehicleCondition { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public string? Address { get; set; }
        public Guid? BrandId { get; set; }
        public string? Brand { get; set; }
        public Guid? ModelId { get; set; }
        public string? Model { get; set; }
        public Guid? CityId { get; set; }
        public string? City { get; set; }
        public Guid? DistrictId { get; set; }
        public string? District { get; set; }
        public Guid? ProvinceId { get; set; }
        public string? Province { get; set; }

        public string? ContactName { get; set; } 
        public string? ContactPhone { get; set; } 
        public string? ContactEmail { get; set; }

        public string? SellerName { get; set; }
        public Guid SellerId { get; set; }
        public string? ReviewerName { get; set; }
        public Guid? ReviewByAdminId { get; set; }
        public DateTime? ReviewedAt { get; set; }

        public bool? IsHidePhone { get; set; } 
        public bool? IsWhatsapp { get; set; } 

        public bool? IsBiddable { get; set; } 
        public string? Status { get; set; } 
        public bool? IsDeleted { get; set; } 
        public string? RejectedReason { get; set; }
    }
}
