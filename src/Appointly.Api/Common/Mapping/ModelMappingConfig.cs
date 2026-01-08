using Appointly.Api.Common.DTOs.Models;
using Appointly.Domain.Entities;
using Mapster;

namespace Appointly.Api.Common.Mapping
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
