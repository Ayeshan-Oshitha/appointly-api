using MotorHub.Application.DTOs.Admin;
using MotorHub.Application.DTOs.Authentication;

namespace MotorHub.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<RegisterResponseDto> Register(RegisterRequestDto request);
        Task<LoginResponseDto> Login(LoginRequestDto request);
        Task<UserResponseDto> GetCurrentUserProfile();
    }
}
