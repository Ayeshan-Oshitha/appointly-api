using Appointly.Application.Common.CurrentUser;
using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Common.Interfaces.Services;
using Appointly.Application.DTOs.Admin;
using Appointly.Application.DTOs.Authentication;
using Appointly.Domain.Infrastructure.Exceptions;

namespace Appointly.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        public AuthenticationService(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository,
            ICurrentUser currentUser)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }
        public async Task<RegisterResponseDto> Register(RegisterRequestDto request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new ConflictException("User with this email already exists.");
            }
            var newUser = await _userRepository.AddUserAsync(request.FirstName, request.LastName, request.Email.ToLower(), request.Password, request.PhoneNumber);

            return new RegisterResponseDto
            {
                UserId = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = request.Email
            };

        }

        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {

            var existingUser = await _userRepository.IsPasswordValid(request.Email.ToLower(), request.Password);

            var token = await _jwtTokenGenerator.GenerateAccessToken(
                existingUser.Id,
                existingUser.IdentityUserId,
                existingUser.FirstName,
                existingUser.LastName,
                request.Email.ToLower());


            return new LoginResponseDto
            {
                UserId = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                Email = request.Email.ToLower(),
                Token = token
            };
        }

        public async Task<UserResponseDto> GetCurrentUserProfile()
        {
            var userId = _currentUser.Id;
            var userProfile = await _userRepository.GetUserProfileByIdAsync(userId);
            return userProfile;
        }
    }
}
