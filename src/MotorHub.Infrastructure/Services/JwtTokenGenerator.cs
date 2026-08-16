using MotorHub.Application.Common.Interfaces.Services;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using MotorHub.Infrastructure.Identity;
using MotorHub.Infrastructure.Services.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MotorHub.Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {

        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        public JwtTokenGenerator(IOptions<JwtSettings> options, UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = options.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateAccessToken(Guid userId, Guid identityUserId, string firstName, string lastName, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var identityUser = await _userManager.FindByIdAsync(identityUserId.ToString());


            if (identityUser == null)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub , userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email.ToLower()),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName , firstName),
                new Claim(JwtRegisteredClaimNames.FamilyName , lastName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
