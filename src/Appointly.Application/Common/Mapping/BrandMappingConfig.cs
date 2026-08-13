using Appointly.Application.DTOs.Brand;
using Appointly.Domain.Entities;
using Mapster;

namespace Appointly.Application.Common.Mapping
{
    public class BrandMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Brand, BrandResponseDto>();
        }
    }
}
