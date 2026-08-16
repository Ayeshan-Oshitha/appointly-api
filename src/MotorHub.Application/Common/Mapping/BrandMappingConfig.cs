using MotorHub.Application.DTOs.Brand;
using MotorHub.Domain.Entities;
using Mapster;

namespace MotorHub.Application.Common.Mapping
{
    public class BrandMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Brand, BrandResponseDto>();
        }
    }
}
