using Appointly.Application.DTOs.Admin;
using Appointly.Application.DTOs.Authentication;

namespace Appointly.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<RegisterResponseDto> Register(RegisterRequestDto request);
        Task<LoginResponseDto> Login(LoginRequestDto request);
        Task<UserResponseDto> GetCurrentUserProfile();
    }
}
