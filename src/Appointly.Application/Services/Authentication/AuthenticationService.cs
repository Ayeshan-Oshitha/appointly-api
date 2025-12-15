using Appointly.Application.Common.Interfaces.Services;
using Appointly.Application.Services.Authentication.Contracts;

namespace Appointly.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public RegisterResponse Register(string firstName, string lastName, string email, string password)
        {
            return new RegisterResponse
            {
                UserId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
        }

        public LoginResponse Login(string email, string password)
        {
            var userId = Guid.NewGuid(); 

            return new LoginResponse
            {
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                Token = _jwtTokenGenerator.GenerateAccessToken(userId, "John" , "Does" , email)
            };
        }
    }
}
