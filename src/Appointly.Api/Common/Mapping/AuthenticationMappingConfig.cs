using Appointly.Api.Common.DTOs.Admin;
using Appointly.Api.Common.DTOs.Authentication;
using Appointly.Application.Services.Admin.Contracts;
using Appointly.Application.Services.Authentication.Contracts;
using Mapster;

namespace Appointly.Api.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<LoginResponse, LoginResponseDto>();
            config.NewConfig<RegisterResponse, RegisterResponseDto>();

            config.NewConfig<UserResponse, UserResponseDto>();
        }
    }
}
