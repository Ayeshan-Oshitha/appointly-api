using MotorHub.Domain.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace MotorHub.Application.DTOs.Advertisements
{
    public class UpdateAdvertisementRequestDto
    {
        [MinLength(3)]
        [MaxLength(100)]
        public string? Title { get; set; }

        [MinLength(10)]
        [MaxLength(2000)]
        public string? Description { get; set; }

        [Range(1950, 2100)]
        public int? Year { get; set; }

        [Range(0.01, 1_000_000_000)]
        public decimal? Price { get; set; }

        [Range(50, 10000)]
        public int? EngineCapacity { get; set; }

        [EnumDataType(typeof(FuelType))]
        public FuelType? FuelType { get; set; }

        [EnumDataType(typeof(TransmissionType))]
        public TransmissionType? TransmissionType { get; set; }

        [EnumDataType(typeof(VehicleCondition))]
        public VehicleCondition? VehicleCondition { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        public Guid? BrandId { get; set; }

        public Guid? ModelId { get; set; }

        public required Guid? CityId { get; set; }

        [MaxLength(100)]
        public string? ContactName { get; set; }

        [Phone]
        public string? ContactPhone { get; set; }

        [EmailAddress]
        public string? ContactEmail { get; set; }

        public bool? IsHidePhone { get; set; }

        public bool? IsWhatsapp { get; set; }

        public bool? IsBiddable { get; set; }
    }
}
