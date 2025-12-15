using Appointly.Application.Services.Authentication.Contracts;

namespace Appointly.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        RegisterResponse Register(string firstName, string lastName, string email, string password);
        LoginResponse Login(string email, string password);
    }
}
