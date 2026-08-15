using MotorHub.Domain.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace MotorHub.Application.DTOs.Advertisements
{
    public class CreateAdvertisementRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public required string Title { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public required string Description { get; set; }

        [Required]
        [Range(1950, 2100)]
        public required int Year { get; set; }

        [Required]
        [Range(0.01, 1_000_000_000)]
        public required decimal Price { get; set; }

        [Range(50, 10000)]
        public int? EngineCapacity { get; set; }

        [Required]
        [EnumDataType(typeof(FuelType))]
        public FuelType? FuelType { get; set; }

        [Required]
        [EnumDataType(typeof(TransmissionType))]
        public TransmissionType? TransmissionType { get; set; }

        [Required]
        [EnumDataType(typeof(VehicleCondition))]
        public VehicleCondition? VehicleCondition { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [Required]
        public required Guid BrandId { get; set; }

        [Required]
        public required Guid ModelId { get; set; }

        [Required]
        public required Guid CityId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string ContactName { get; set; }

        [Required]
        [Phone]
        public required string ContactPhone { get; set; }

        [Required]
        [EmailAddress]
        public required string ContactEmail { get; set; }

        public bool IsHidePhone { get; set; }

        public bool? IsWhatsapp { get; set; }

        public bool IsBiddable { get; set; }
    }
}
