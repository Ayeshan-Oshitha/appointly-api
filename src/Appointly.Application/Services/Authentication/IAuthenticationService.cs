using Appointly.Application.Services.Authentication.Contracts;

namespace Appointly.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> Register(string firstName, string lastName, string email, string password);
        Task<LoginResponse> Login(string email, string password);
    }
}
