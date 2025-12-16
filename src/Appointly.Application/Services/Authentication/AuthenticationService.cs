using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Common.Interfaces.Services;
using Appointly.Application.Services.Authentication.Contracts;
using System.Threading.Tasks;

namespace Appointly.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IuserRepository _userRepository;
        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IuserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }
        public async Task<RegisterResponse> Register(string firstName, string lastName, string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }
            var newUser =await _userRepository.AddUserAsync(firstName, lastName, email, password);

            return new RegisterResponse
            {
                UserId = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email
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
