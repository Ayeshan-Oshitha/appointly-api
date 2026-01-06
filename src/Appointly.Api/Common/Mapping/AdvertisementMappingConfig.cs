using Appointly.Api.Common.DTOs.Advertisment;
using Appointly.Application.Services.Advertisment.Contracts;
using Appointly.Domain.Entities;
using Mapster;

namespace Appointly.Api.Common.Mapping
{
    public class AdvertisementMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateAdvertisementRequestDto, CreateAdvertisementRequest>();

            config.NewConfig<Advertisement, AdvertisementResponseDto>();

            config.NewConfig<Advertisement, AdvertisementDetailResponseDto>()
                .Map(dest => dest.Brand, src => src.Brand.Name)
                .Map(dest => dest.Model, src => src.Model.Name)
                .Map(dest => dest.City, src => src.City.Name)
                .Map(dest => dest.District, src => src.City.District.Name)
                .Map(dest => dest.Province, src => src.City.Province.Name)
                .Map(dest => dest.DistrictId, src => src.City.DistrictId)
                .Map(dest => dest.ProvinceId, src => src.City.ProvinceId);

        }
    }
}
