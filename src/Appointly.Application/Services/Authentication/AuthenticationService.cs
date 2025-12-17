using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Common.Interfaces.Services;
using Appointly.Application.Services.Authentication.Contracts;
using Appointly.Domain.Infrastructure.Exceptions;
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
            var newUser =await _userRepository.AddUserAsync(firstName.ToLower(), lastName.ToLower(), email.ToLower(), password);

            return new RegisterResponse
            {
                UserId = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email
            };

        }

        public async Task<LoginResponse> Login(string email, string password)
        {

            var existingUser = await _userRepository.GetUserByEmailAsync(email);

            if(existingUser == null || existingUser.PasswordHash != password)
            {
                throw new NotFoundException("Invalid email or password.");

            }

            return new LoginResponse
            {
                UserId = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                Email = existingUser.Email,
                Token = _jwtTokenGenerator.GenerateAccessToken(
                    existingUser.Id,
                    existingUser.FirstName,
                    existingUser.LastName,
                    existingUser.Email)
            };
        }

       
    }
}
