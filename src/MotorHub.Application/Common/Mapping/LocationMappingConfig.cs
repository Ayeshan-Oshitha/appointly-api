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

            config.NewConfig<District, DistrictResponseDto>()
                .Map(dest => dest.Province, src => src.Province.Name);

            config.NewConfig<City, CityResponseDto>()
                .Map(dest => dest.Province, src => src.Province.Name)
                .Map(dest => dest.District, src => src.District.Name);
        }
    }
}
