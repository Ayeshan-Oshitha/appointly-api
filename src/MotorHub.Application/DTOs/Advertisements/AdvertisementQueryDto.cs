using MotorHub.Domain.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace MotorHub.Application.DTOs.Advertisements
{
    public class AdvertisementQueryDto
    {
        public string? Search { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinEngineCapacity { get; set; }
        public int? MaxEngineCapacity { get; set; }

        [EnumDataType(typeof(FuelType))]
        public FuelType? FuelType { get; set; }

        [EnumDataType(typeof(TransmissionType))]
        public TransmissionType? TransmissionType { get; set; }

        [EnumDataType(typeof(VehicleCondition))]
        public VehicleCondition? VehicleCondition { get; set; }
        public SortBy? SortBy { get; set; }
        public SortOrder? SortOrder { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ModelId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? ProvinceId { get; set; }
        public bool? IsBiddable { get; set; }

        [EnumDataType(typeof(AdStatus))]
        public AdStatus? Status { get; set; }
    }
}
