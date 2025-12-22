using Appointly.Application.Services.Authentication.Contracts;
using Appointly.Api.Common.DTOs.Authentication;
using Mapster;

namespace Appointly.Api.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<LoginResponse, LoginResponseDto>();
            config.NewConfig<RegisterResponse, RegisterResponseDto>();
        }
    }
}
