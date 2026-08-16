using MotorHub.Application.DTOs.Location;
using MotorHub.Domain.Entities;
using Mapster;

namespace MotorHub.Application.Common.Mapping
{
    public class LocationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Province, ProvinceResponseDto>();

            // Navigations are guarded rather than dereferenced directly, matching
            // AdvertisementMappingConfig. Callers are expected to load them (GetCityById and
            // GetDistrictsByProvinceIdAsync Include them), but a guard turns a missed Include into
            // a null field instead of a 500 from inside the mapper.
            config.NewConfig<District, DistrictResponseDto>()
                .Map(dest => dest.Province, src => src.Province != null ? src.Province.Name : null);

            config.NewConfig<City, CityResponseDto>()
                .Map(dest => dest.Province, src => src.Province != null ? src.Province.Name : null)
                .Map(dest => dest.District, src => src.District != null ? src.District.Name : null);
        }
    }
}
