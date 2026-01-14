using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Common.Interfaces.Services;
using Appointly.Application.Services.Authentication.Contracts;
using Appointly.Domain.Infrastructure.Exceptions;


namespace Appointly.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }
        public async Task<RegisterResponse> Register(string firstName, string lastName, string email, string password, string phoneNumber)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                throw new ConflictException("User with this email already exists.");
            }
            var newUser = await _userRepository.AddUserAsync(firstName, lastName, email.ToLower(), password, phoneNumber);

            return new RegisterResponse
            {
                UserId = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = email
            };

        }

        public async Task<LoginResponse> Login(string email, string password)
        {

            var existingUser = await _userRepository.IsPasswordValid(email.ToLower(), password);

            var token = _jwtTokenGenerator.GenerateAccessToken(
                existingUser.Id,
                existingUser.FirstName,
                existingUser.LastName,
                email.ToLower());


            return new LoginResponse
            {
                UserId = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                Email = email.ToLower(),
                Token = token
            };
        }

       
    }
}
