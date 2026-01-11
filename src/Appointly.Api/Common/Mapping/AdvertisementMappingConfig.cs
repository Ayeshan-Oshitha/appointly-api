using Appointly.Api.Common.DTOs.Advertisment;
using Appointly.Application.Services.Advertisment.Contracts;
using Appointly.Application.Services.Advertisments.Contracts;
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
                .Map(dest => dest.Brand, src => src.Brand != null ? src.Brand.Name : null)
                .Map(dest => dest.Model, src => src.Model != null ? src.Model.Name : null)
                .Map(dest => dest.City, src => src.City != null ? src.City.Name : null)
                .Map(dest => dest.District, src => src.City != null && src.City.District != null ? src.City.District.Name : null)
                .Map(dest => dest.Province, src => src.City != null && src.City.Province != null ? src.City.Province.Name : null)
                .Map(dest => dest.DistrictId, src => src.City != null ? src.City.DistrictId : (Guid?)null)
                .Map(dest => dest.ProvinceId, src => src.City != null ? src.City.ProvinceId : (Guid?)null);

            config.NewConfig<UpdateAdvertisementRequestDto, UpdateAdvertisementRequest>();

            config.NewConfig<UpdateAdvertisementRequest, Advertisement>()
                .IgnoreNullValues(true);

            config.NewConfig<AdvertisementQueryDto, AdvertisementQuery>();
        }
    }
}
