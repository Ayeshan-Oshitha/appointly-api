using Appointly.Application.DTOs.Advertisements;
using Appointly.Domain.Entities;
using Mapster;

namespace Appointly.Application.Common.Mapping
{
    public class AdvertisementMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Advertisement, AdvertisementResponseDto>();

            config.NewConfig<Advertisement, AdvertisementDetailResponseDto>()
                .Map(dest => dest.Brand, src => src.Brand != null ? src.Brand.Name : null)
                .Map(dest => dest.Model, src => src.Model != null ? src.Model.Name : null)
                .Map(dest => dest.City, src => src.City != null ? src.City.Name : null)
                .Map(dest => dest.District, src => src.City != null && src.City.District != null ? src.City.District.Name : null)
                .Map(dest => dest.Province, src => src.City != null && src.City.Province != null ? src.City.Province.Name : null)
                .Map(dest => dest.DistrictId, src => src.City != null ? src.City.DistrictId : (Guid?)null)
                .Map(dest => dest.ProvinceId, src => src.City != null ? src.City.ProvinceId : (Guid?)null)
                .Map(dest => dest.SellerName, src => src.Seller != null ? $"{src.Seller.FirstName} {src.Seller.LastName}" : null)
                .Map(dest => dest.ReviewerName, src => src.ReviewByAdmin != null ? $"{src.ReviewByAdmin.FirstName} {src.ReviewByAdmin.LastName}" : null);

            config.NewConfig<UpdateAdvertisementRequestDto, Advertisement>()
                .IgnoreNullValues(true);
        }
    }
}
