using Appointly.Api.Common.DTOs.Brands;
using Appointly.Domain.Entities;
using Mapster;

namespace Appointly.Api.Common.Mapping
{
    public class BrandMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Brand, BrandResponseDto>();
        }
    }
}
