using MotorHub.Application.DTOs.Model;
using MotorHub.Domain.Entities;
using Mapster;

namespace MotorHub.Application.Common.Mapping
{
    public class ModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Model, ModelResponseDto>()
                .Map(dest => dest.Brand, src => src.Brand.Name);
        }
    }
}
