using Appointly.Api.Common.DTOs.Location;
using Appointly.Domain.Entities;
using Mapster;

namespace Appointly.Api.Common.Mapping
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
