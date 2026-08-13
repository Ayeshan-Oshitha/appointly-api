using Appointly.Application.DTOs.Model;
using Appointly.Domain.Entities;
using Mapster;

namespace Appointly.Application.Common.Mapping
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
