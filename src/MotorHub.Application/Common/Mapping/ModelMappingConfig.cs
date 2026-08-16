using MotorHub.Application.DTOs.Model;
using MotorHub.Domain.Entities;
using Mapster;

namespace MotorHub.Application.Common.Mapping
{
    public class ModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Guarded rather than dereferenced directly; see LocationMappingConfig for why.
            config.NewConfig<Model, ModelResponseDto>()
                .Map(dest => dest.Brand, src => src.Brand != null ? src.Brand.Name : null);
        }
    }
}
