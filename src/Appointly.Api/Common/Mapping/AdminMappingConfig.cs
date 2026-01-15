using Appointly.Api.Common.DTOs.Admin;
using Appointly.Application.Services.Admin.Contracts;
using Mapster;

namespace Appointly.Api.Common.Mapping
{
    public class AdminMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserResponse, UserResponseDto>();
        }
    }
}
